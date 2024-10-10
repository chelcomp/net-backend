using InterviewAssignment.Business;
using InterviewAssignment.Database.Repositories.DbOperation.Models;

namespace InterviewAssignment.Unit.Tests.Business
{
    public class ServicesTests
    {
        private readonly Services _services;

        public ServicesTests()
        {
            _services = new Services();
        }

        [Theory]
        [InlineData(10, 2, "division", 5)]
        [InlineData(10, 5, "multiplication", 50)]
        [InlineData(10, 5, "addition", 15)]
        [InlineData(10, 5, "subtraction", 5)]
        [InlineData(10, 3, "remainder", 1)]
        public void Calculate_ValidOperations_ReturnsExpectedResult(double left, double right, string operation, double expected)
        {
            // Arrange
            var modelRequest = new OperationModel
            {
                Left = left,
                Right = right,
                Operation = operation
            };

            // Act
            var result = _services.Calculate(modelRequest);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Calculate_InvalidOperation_ReturnsZero()
        {
            // Arrange
            var modelRequest = new OperationModel
            {
                Left = 10,
                Right = 5,
                Operation = "invalid_operation"
            };

            // Act
            var result = _services.Calculate(modelRequest);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Calculate_DivisionByZero_Infinity()
        {
            // Arrange
            var modelRequest = new OperationModel
            {
                Left = 10,
                Right = 0,
                Operation = "division"
            };

            // Act & Assert
            var act = _services.Calculate(modelRequest);
            
            Assert.True(double.IsInfinity(act));
        }
    }
}