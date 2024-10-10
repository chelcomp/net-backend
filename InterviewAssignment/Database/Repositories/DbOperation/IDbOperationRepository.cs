using InterviewAssignment.Database.Repositories.DbOperation.Models;

namespace InterviewAssignment.Database.Repositories.DbOperation;

public interface IDbOperationRepository
{
    Task<bool> CreateAsync(OperationModel dbEntity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<OperationModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<OperationModel?> GetFirstAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(OperationModel dbEntity, CancellationToken cancellationToken);
}