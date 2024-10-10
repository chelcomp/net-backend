
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;

namespace InterviewAssignment.Database.Repositories.DbOperationResult;

public class EnqueueDbOperationResultRepository : IEnqueueDbOperationResultRepository
{
    private readonly IDbOperationResultRepository _operationResultRepository;

    public EnqueueDbOperationResultRepository(IDbOperationResultRepository operationResultRepository)
    {
        _operationResultRepository = operationResultRepository;
    }

    public Task CreateAsync(OperationResultModel db, CancellationToken cancellationToken)
    {
        // TODO: Implement a async/offline database persistence using any queue engine or any other background service technique.
        _operationResultRepository.CreateAsync(db, cancellationToken);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement a async/offline database persistence using any queue engine or any other background service technique.
        _operationResultRepository.DeleteAsync(id, cancellationToken);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(OperationResultModel db, CancellationToken cancellationToken)
    {
        // TODO: Implement a async/offline database persistence using any queue engine or any other background service technique.
        _operationResultRepository.UpdateAsync(db, cancellationToken);
        return Task.CompletedTask;
    }
}