using System.Data;
using InterviewAssignment.Database;
using InterviewAssignment.Database.Repositories.DbOperationResult;
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;
using InterviewAssignment.Database.Setup;

namespace InterviewAssignment.Unit.Tests.Database.Repositories.DbOperationResult
{
    public class DbOperationResultRepositoryTests:IDisposable
    {
        private readonly DbOperationResultRepository _repository;
        private string _databseFileFullPathName= $"./../../../../InterviewAssignment/database{Guid.NewGuid().ToString()}.sqlite";

        public DbOperationResultRepositoryTests()
        {
            // Create an in-memory SQLite connection
            var connectionFactory = new SqliteConnectionFactory($"Data Source={_databseFileFullPathName}");;
            var dbInitializer = new DatabaseInitializer(connectionFactory);
            dbInitializer.InitializeAsync().Wait();

            _repository = new DbOperationResultRepository(connectionFactory);
        }

        [Fact]
        public async Task CreateAsync_ValidOperationResultModel_ReturnsTrue()
        {
            // Arrange
            var operationResultModel = new OperationResultModel
            {
                Id = Guid.NewGuid().ToString(),
                Result = 42.0,
                Description = "Result of addition",
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Act
            var result = await _repository.CreateAsync(operationResultModel, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOperationResults()
        {
            // Arrange
            var operationResultModel1 = new OperationResultModel
            {
                Id = Guid.NewGuid().ToString(),
                Result = 42.0,
                Description = "Result of addition",
                CorrelationId = Guid.NewGuid().ToString()
            };
            var operationResultModel2 = new OperationResultModel
            {
                Id = Guid.NewGuid().ToString(),
                Result = 15.0,
                Description = "Result of subtraction",
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationResultModel1, CancellationToken.None);
            await _repository.CreateAsync(operationResultModel2, CancellationToken.None);

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAsync_ValidId_ReturnsOperationResultModel()
        {
            // Arrange
            var operationResultModel = new OperationResultModel
            {
                Id = Guid.NewGuid().ToString(),
                Result = 42.0,
                Description = "Result of addition",
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationResultModel, CancellationToken.None);

            // Act
            var result = await _repository.GetAsync(Guid.Parse(operationResultModel.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(operationResultModel.Id, result?.Id);
            Assert.Equal(operationResultModel.Result, result?.Result);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var operationResultModel = new OperationResultModel
            {
                Id = Guid.NewGuid().ToString(),
                Result = 42.0,
                Description = "Result of addition",
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationResultModel, CancellationToken.None);

            // Act
            var result = await _repository.DeleteAsync(Guid.Parse(operationResultModel.Id), CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ValidOperationResultModel_ReturnsTrue()
        {
            // Arrange
            var operationResultModel = new OperationResultModel
            {
                Id = Guid.NewGuid().ToString(),
                Result = 42.0,
                Description = "Result of addition",
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationResultModel, CancellationToken.None);

            // Update the result
            operationResultModel.Result = 100.0;
            operationResultModel.Description = "Updated result";

            // Act
            var result = await _repository.UpdateAsync(operationResultModel, CancellationToken.None);

            // Assert
            Assert.True(result);

            // Verify the update
            var updatedModel = await _repository.GetAsync(Guid.Parse(operationResultModel.Id), CancellationToken.None);
            Assert.Equal(100.0, updatedModel?.Result);
            Assert.Equal("Updated result", updatedModel?.Description);
        }
        
        // Helper class for creating an in-memory DB connection
        private class InMemoryDbConnectionFactory : IDbConnectionFactory
        {
            private readonly IDbConnection _connection;

            public InMemoryDbConnectionFactory(IDbConnection connection)
            {
                _connection = connection;
            }

            public Task<IDbConnection> CreateConnectionAsync()
            {
                return Task.FromResult(_connection);
            }
        }

        public void Dispose()
        {
           // File.Delete(_databseFileFullPathName);
        }
    }
}
