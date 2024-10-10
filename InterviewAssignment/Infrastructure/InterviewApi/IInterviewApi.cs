
using InterviewAssignment.Infrastructure.InterviewApi.Domain;

namespace InterviewAssignment.Infrastructure.InterviewApi;

public interface IInterviewApi
{
    Task<string> SubmitTaskAsync(SubmitTaskRequest submitTaskRequest, CancellationToken cancellationToken);
    Task<GetTaskResponse> GetTaskAsync(CancellationToken cancellationToken);
}