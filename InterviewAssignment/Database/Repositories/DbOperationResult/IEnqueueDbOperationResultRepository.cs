
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;

namespace InterviewAssignment.Database.Repositories.DbOperationResult;

public interface IEnqueueDbOperationResultRepository
{
    Task CreateAsync(OperationResultModel db, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(OperationResultModel db, CancellationToken cancellationToken);
}