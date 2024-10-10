using InterviewAssignment.Database.Repositories.DbOperationResult.Models;

namespace InterviewAssignment.Database.Repositories.DbOperationResult;

public interface IDbOperationResultRepository
{
    Task<bool> CreateAsync(OperationResultModel db, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<OperationResultModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<OperationResultModel?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(OperationResultModel db, CancellationToken cancellationToken);
}