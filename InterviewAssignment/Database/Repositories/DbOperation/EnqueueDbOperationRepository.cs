using InterviewAssignment.Database.Repositories.DbOperation.Models;

namespace InterviewAssignment.Database.Repositories.DbOperation;

public class EnqueueDbOperationRepository : IEnqueueDbOperationRepository
{
    private readonly IDbOperationRepository _dbOperationRepository;

    public EnqueueDbOperationRepository(IDbOperationRepository dbOperationRepository)
    {
        _dbOperationRepository = dbOperationRepository;
    }

    public Task CreateAsync(OperationModel dbEntity, CancellationToken cancellationToken)
    {
        _dbOperationRepository.CreateAsync(dbEntity, cancellationToken);
        return Task.CompletedTask;
    }
}