using InterviewAssignment.Database.Repositories.DbOperation.Models;

namespace InterviewAssignment.Database.Repositories.DbOperation;

public interface IEnqueueDbOperationRepository
{
    Task CreateAsync(OperationModel dbEntity, CancellationToken cancellationToken);
}