using Dapper;
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;

namespace InterviewAssignment.Database.Repositories.DbOperationResult;

public class DbOperationResultRepository : IDbOperationResultRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbOperationResultRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<bool> CreateAsync(OperationResultModel db, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO OperationResult
                              (Id, Result,Description,CorrelationId)
                 VALUES (@Id, @Result,@Description,@CorrelationId)",
                db,
                cancellationToken: cancellationToken));

        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            new CommandDefinition(
                @"DELETE FROM OperationResult WHERE Id = @Id",
                new { Id = id.ToString() },
                cancellationToken: cancellationToken));

        return result > 0;
    }

    public async Task<IEnumerable<OperationResultModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<OperationResultModel>(
            new CommandDefinition(
                "SELECT * FROM OperationResult",
                cancellationToken: cancellationToken));
    }

    public async Task<OperationResultModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<OperationResultModel>(
            new CommandDefinition(
                "SELECT * FROM OperationResult WHERE Id = @Id LIMIT 1",
                new { Id = id.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(OperationResultModel db, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            new CommandDefinition(
                @"UPDATE OperationResult SET Result=@Result,Description=@Description,CorrelationId=@CorrelationId WHERE Id = @Id",
                db,
                cancellationToken: cancellationToken));

        return result > 0;
    }
}