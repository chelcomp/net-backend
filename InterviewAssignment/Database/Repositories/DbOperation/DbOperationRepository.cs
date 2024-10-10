using Dapper;
using InterviewAssignment.Database.Repositories.DbOperation.Models;

namespace InterviewAssignment.Database.Repositories.DbOperation;

public class DbOperationRepository : IDbOperationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbOperationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<bool> CreateAsync(OperationModel dbEntity, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO Operation
                              (Id, Operation,Left, Right, CorrelationId) 
                 VALUES (@Id, @Operation,@Left, @Right, @CorrelationId)",
                dbEntity,
                cancellationToken: cancellationToken));

        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            new CommandDefinition(
                @"DELETE FROM Operation WHERE Id = @Id",
                new { Id = id.ToString() },
                cancellationToken: cancellationToken));

        return result > 0;
    }

    public async Task<IEnumerable<OperationModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<OperationModel>(
            new CommandDefinition(
                "SELECT * FROM Operation",
                cancellationToken: cancellationToken));
    }

    public async Task<OperationModel?> GetFirstAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<OperationModel>(
            new CommandDefinition(
                "SELECT * FROM Operation WHERE Id = @Id LIMIT 1",
                new { Id = id.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(OperationModel dbEntity, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            new CommandDefinition(
                @"UPDATE Operation SET Operation=@Operation,Left=@Left, Right=@Right, CorrelationId=@CorrelationId WHERE Id = @Id",
                dbEntity,
                cancellationToken: cancellationToken));

        return result > 0;
    }
}