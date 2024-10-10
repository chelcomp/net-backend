using System.Text.Json;
using InterviewAssignment.Infrastructure.InterviewApi;
using InterviewAssignment.Infrastructure.InterviewApi.Domain;
using Microsoft.Extensions.Options;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace InterviewAssignment.Unit.Tests.Infrastructure.InterviewApi
{
    public class InterviewApiWireMockTests : IDisposable
    {
        private readonly WireMockServer _wireMockServer;
        private readonly InterviewAssignment.Infrastructure.InterviewApi.InterviewApi _sut; // SUT (System Under Test)
        private readonly IOptions<InterviewApiOptions> _mockOptions;

        public InterviewApiWireMockTests()
        {
            // Initialize WireMock server
            _wireMockServer = WireMockServer.Start();

            // Configure mocked InterviewApiOptions
            _mockOptions = Options.Create(new InterviewApiOptions
            {
                SubmitTask = $"{_wireMockServer.Url}/submit",
                GetTask = $"{_wireMockServer.Url}/gettask"
            });

            // Set up the HttpClient to point to the WireMock server
            var httpClient = new HttpClient { BaseAddress = new Uri(_wireMockServer.Url) };

            // Initialize SUT (InterviewApi) with mocked dependencies
            _sut = new InterviewAssignment.Infrastructure.InterviewApi.InterviewApi(_mockOptions, httpClient);
        }

        [Fact]
        public async Task SubmitTaskAsync_ValidRequest_ReturnsExpectedResponse()
        {
            // Arrange
            var submitTaskRequest = new SubmitTaskRequest { Id = "ghi-789", Result = 42 };
            var expectedResponse = "Task submitted successfully";

            // WireMock: Set up the mock endpoint to respond with the expected result
            _wireMockServer
                .Given(Request.Create().WithPath("/submit").UsingPost())
                .RespondWith(Response.Create().WithStatusCode(200).WithBody(expectedResponse));

            // Act
            var result = await _sut.SubmitTaskAsync(submitTaskRequest, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetTaskAsync_ValidResponse_ReturnsGetTaskResponse()
        {
            // Arrange
            var expectedTaskResponse = new GetTaskResponse { Id = "ghi-789", Left = 10, Right = 5, Operation = "addition" };
            var mockResponseString = JsonSerializer.Serialize(expectedTaskResponse);

            // WireMock: Set up the mock endpoint to respond with the task JSON
            _wireMockServer
                .Given(Request.Create().WithPath("/gettask").UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBody(mockResponseString));

            // Act
            var result = await _sut.GetTaskAsync(CancellationToken.None);

            // Assert
            Assert.Equal(expectedTaskResponse.Id, result.Id);
            Assert.Equal(expectedTaskResponse.Left, result.Left);
            Assert.Equal(expectedTaskResponse.Right, result.Right);
            Assert.Equal(expectedTaskResponse.Operation, result.Operation);
        }

        [Fact]
        public async Task SubmitTaskAsync_HttpClientReturnsError_ReturnsErrorMessage()
        {
            // Arrange
            var submitTaskRequest = new SubmitTaskRequest { Id = "ghi-789", Result = 42 };

            // WireMock: Simulate a BadRequest response
            _wireMockServer
                .Given(Request.Create().WithPath("/submit").UsingPost())
                .RespondWith(Response.Create().WithStatusCode(400).WithBody("Bad request"));

            // Act
            var result = await _sut.SubmitTaskAsync(submitTaskRequest, CancellationToken.None);

            // Assert
            Assert.Equal("Bad request", result);
        }

        public void Dispose()
        {
            // Stop WireMock server after tests
            _wireMockServer.Stop();
            _wireMockServer.Dispose();
        }
    }
}