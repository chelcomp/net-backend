using System.Data;
using Dapper;
using InterviewAssignment.Database;
using InterviewAssignment.Database.Repositories.DbOperation;
using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Setup;
using Microsoft.Data.Sqlite;

namespace InterviewAssignment.Unit.Tests.Database.Repositories.DbOperation
{
    public class DbOperationRepositoryTests :IDisposable
    {
        private readonly DbOperationRepository _repository;
        private string _databseFileFullPathName= $"./../../../../InterviewAssignment/database{Guid.NewGuid().ToString()}.sqlite";

        public DbOperationRepositoryTests()
        {
            // Create an in-memory SQLite connection
            var connectionFactory = new SqliteConnectionFactory($"Data Source={_databseFileFullPathName}");;
            var dbInitializer = new DatabaseInitializer(connectionFactory);
            dbInitializer.InitializeAsync().Wait();
            
            _repository = new DbOperationRepository(connectionFactory);
        }

        [Fact]
        public async Task CreateAsync_ValidOperationModel_ReturnsTrue()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Operation = "addition",
                Left = 5,
                Right = 10,
                CorrelationId = Guid.NewGuid().ToString()
            };

            // Act
            var result = await _repository.CreateAsync(operationModel, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOperations()
        {
            // Arrange
            var operationModel1 = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Operation = "addition",
                Left = 5,
                Right = 10,
                CorrelationId = Guid.NewGuid().ToString()
            };
            var operationModel2 = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Operation = "subtraction",
                Left = 15,
                Right = 5,
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationModel1, CancellationToken.None);
            await _repository.CreateAsync(operationModel2, CancellationToken.None);

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFirstAsync_ValidId_ReturnsOperationModel()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Operation = "addition",
                Left = 5,
                Right = 10,
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationModel, CancellationToken.None);

            // Act
            var result = await _repository.GetFirstAsync(Guid.Parse(operationModel.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(operationModel.Id, result.Id);
            Assert.Equal(operationModel.Operation, result.Operation);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Operation = "addition",
                Left = 5,
                Right = 10,
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationModel, CancellationToken.None);

            // Act
            var result = await _repository.DeleteAsync(Guid.Parse(operationModel.Id), CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ValidOperationModel_ReturnsTrue()
        {
            // Arrange
            var operationModel = new OperationModel
            {
                Id = Guid.NewGuid().ToString(),
                Operation = "addition",
                Left = 5,
                Right = 10,
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _repository.CreateAsync(operationModel, CancellationToken.None);

            // Update the operation
            operationModel.Operation = "subtraction";

            // Act
            var result = await _repository.UpdateAsync(operationModel, CancellationToken.None);

            // Assert
            Assert.True(result);

            // Verify the update
            var updatedModel = await _repository.GetFirstAsync(Guid.Parse(operationModel.Id), CancellationToken.None);
            Assert.Equal("subtraction", updatedModel?.Operation);
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
            //File.Delete(_databseFileFullPathName);
        }
    }
}
