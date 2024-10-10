using InterviewAssignment.Business;
using InterviewAssignment.Controllers;
using InterviewAssignment.Database.Repositories.DbOperation;
using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Repositories.DbOperationResult;
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;
using InterviewAssignment.Infrastructure.InterviewApi;
using InterviewAssignment.Infrastructure.InterviewApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InterviewAssignment.Unit.Tests.Controllers
{
    public class ProcessControllerTests
    {
        private readonly Mock<IServices> _mockServices;
        private readonly Mock<IEnqueueDbOperationResultRepository> _mockEnqueueDbOperationResultRepository;
        private readonly Mock<IEnqueueDbOperationRepository> _mockEnqueueDbOperationRepository;
        private readonly Mock<IInterviewApi> _mockInterviewApi;

        public ProcessControllerTests()
        {
            _mockServices = new Mock<IServices>();
            _mockEnqueueDbOperationResultRepository = new Mock<IEnqueueDbOperationResultRepository>();
            _mockEnqueueDbOperationRepository = new Mock<IEnqueueDbOperationRepository>();
            _mockInterviewApi = new Mock<IInterviewApi>();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var mockGetTaskResponse = new GetTaskResponse
            {
                Id = Guid.NewGuid().ToString(),
                Left = 10,
                Right = 5,
                Operation = "addition",
                CorrelationId = null
            };
            _mockInterviewApi.Setup(api => api.GetTaskAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockGetTaskResponse);

            // SUT (System Under Test)
            var sut = new Process(
                _mockServices.Object,
                _mockEnqueueDbOperationResultRepository.Object,
                _mockInterviewApi.Object,
                _mockEnqueueDbOperationRepository.Object);

            // Act
            var result = await sut.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GetTaskResponse>(okResult.Value);
            Assert.Equal(mockGetTaskResponse.Id, response.Id);
            Assert.Equal(mockGetTaskResponse.Operation, response.Operation);
            Assert.Equal(mockGetTaskResponse.CorrelationId, response.CorrelationId);
            _mockEnqueueDbOperationRepository.Verify(repo => repo.CreateAsync(It.IsAny<OperationModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockInterviewApi.Setup(api => api.GetTaskAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            // SUT
            var sut = new Process(
                _mockServices.Object,
                _mockEnqueueDbOperationResultRepository.Object,
                _mockInterviewApi.Object,
                _mockEnqueueDbOperationRepository.Object);

            // Act
            var result = await sut.GetAll(CancellationToken.None);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Left = 10,
                Right = 5,
                Operation = "addition",
                CorrelationId = Guid.NewGuid().ToString()
            };
            var resultCalculated = 15.0;
            _mockServices.Setup(s => s.Calculate(operationModel)).Returns(resultCalculated);

            var submitTaskResponse = "Task Submitted Successfully";
            _mockInterviewApi.Setup(api => api.SubmitTaskAsync(It.IsAny<SubmitTaskRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(submitTaskResponse);

            // SUT
            var sut = new Process(
                _mockServices.Object,
                _mockEnqueueDbOperationResultRepository.Object,
                _mockInterviewApi.Object,
                _mockEnqueueDbOperationRepository.Object);

            // Act
            var result = await sut.Post(operationModel, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(submitTaskResponse, okResult.Value);
            _mockEnqueueDbOperationResultRepository.Verify(repo => repo.CreateAsync(It.IsAny<OperationResultModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Left = 10,
                Right = 5,
                Operation = "addition",
                CorrelationId = Guid.NewGuid().ToString()
            };

            _mockServices.Setup(s => s.Calculate(operationModel)).Throws(new Exception("Calculation error"));

            // SUT
            var sut = new Process(
                _mockServices.Object,
                _mockEnqueueDbOperationResultRepository.Object,
                _mockInterviewApi.Object,
                _mockEnqueueDbOperationRepository.Object);

            // Act
            var result = await sut.Post(operationModel, CancellationToken.None);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
