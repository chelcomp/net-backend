using Dapper;

namespace InterviewAssignment.Database.Setup;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Operation (
                                        Id uuid PRIMARY KEY,
                                        Operation TEXT NOT NULL,
                                        Left REAL NOT NULL,
                                        Right REAL NOT NULL,
                                        CorrelationId uuid NOT NULL
                                       )");

        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS OperationResult (
                                        Id uuid ,
                                        Result REAL NOT NULL,     
                                        Description TEXT NOT NULL,
                                        CorrelationId uuid NOT NULL
                                       )");
    }
}