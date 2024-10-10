using InterviewAssignment.Controllers.Maps;
using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;
using InterviewAssignment.Infrastructure.InterviewApi.Domain;

namespace InterviewAssignment.Unit.Tests.Controllers.Maps
{
    public class ProfileMapTests
    {
        [Fact]
        public void MapToOperationView_OperationModels_ShouldMapToOperationView()
        {
            // Arrange
            var operationModels = new List<OperationModel>
            {
                new OperationModel { Id = "abc-123", Left = 5, Right = 3, Operation = "Sum", CorrelationId = "123" },
                new OperationModel
                    { Id = "def-456", Left = 7, Right = 2, Operation = "Subtraction", CorrelationId = "456" }
            };

            // Act
            var result = operationModels.MapToOperationView();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("abc-123", result.First().id);
            Assert.Equal(5, result.First().Left);
            Assert.Equal("Sum", result.First().Operation);
        }

        [Fact]
        public void MapToOperationView_OperationResultModels_ShouldMapToOperationResultView()
        {
            // Arrange
            var operationResultModels = new List<OperationResultModel>
            {
                new OperationResultModel
                    { Id = "xyz-789", Description = "Success", Result = 8.0, CorrelationId = "789" },
                new OperationResultModel
                    { Id = "uvw-012", Description = "Failed", Result = -5.0, CorrelationId = "012" }
            };

            // Act
            var result = operationResultModels.MapToOperationView();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Success", result.First().Description);
            Assert.Equal(8.0, result.First().Result);
        }

        [Fact]
        public void MapToOperationModel_GetTaskResponse_ShouldMapToOperationModel()
        {
            // Arrange
            var getTaskResponse = new GetTaskResponse
            {
                Id = "abc-123",
                CorrelationId = "999",
                Left = 10,
                Right = 5,
                Operation = "Sum"
            };

            // Act
            var result = getTaskResponse.MapToOperationModel();

            // Assert
            Assert.Equal("abc-123", result.Id);
            Assert.Equal(10, result.Left);
            Assert.Equal("Sum", result.Operation);
        }

        [Fact]
        public void MapToOperationResultModel_OperationModel_ShouldMapToOperationResultModel()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = "ghi-789",
                CorrelationId = "888",
                Left = 7,
                Right = 3,
                Operation = "Multiplication"
            };

            var resultCalculated = 21.0;
            var responseString = "Multiplication successful";
            var initialCorrelationId = "test-id";

            // Act
            var result =
                operationModel.MapToOperationResultModel(resultCalculated, responseString, initialCorrelationId);

            // Assert
            Assert.Equal("ghi-789", result.Id);
            Assert.Equal(21.0, result.Result);
            Assert.Equal("Multiplication successful", result.Description);
            Assert.Equal("test-id", result.CorrelationId);
        }

        [Fact]
        public void MapToOperationResultView_OperationModel_ShouldMapToSubmitTaskRequest()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = "jkl-123",
                Left = 10,
                Right = 5,
                Operation = "Sum"
            };
            var resultCalculated = 15.0;

            // Act
            var result = operationModel.MapToOperationResultView(resultCalculated);

            // Assert
            Assert.Equal("jkl-123", result.Id);
            Assert.Equal(15.0, result.Result);
        }
    }
}

