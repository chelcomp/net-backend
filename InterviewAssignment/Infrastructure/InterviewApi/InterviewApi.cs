using System.Text;
using InterviewAssignment.Infrastructure.InterviewApi.Domain;
using Microsoft.Extensions.Options;

namespace InterviewAssignment.Infrastructure.InterviewApi;

public class InterviewApi : IInterviewApi
{
    private readonly IOptions<InterviewApiOptions> _interviewApiOptions;
    private readonly HttpClient _httpClient;

    public InterviewApi(IOptions<InterviewApiOptions> interviewApiOptions,
        HttpClient httpClient)
    {
        _interviewApiOptions = interviewApiOptions;
        _httpClient = httpClient;
    }

    public async Task<string> SubmitTaskAsync(SubmitTaskRequest submitTaskRequest, CancellationToken cancellationToken)
    {
        var request = System.Text.Json.JsonSerializer.Serialize(submitTaskRequest);
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_interviewApiOptions.Value.SubmitTask, content, cancellationToken);
        var responseString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        return responseString;
    }

    public async Task<GetTaskResponse> GetTaskAsync(CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetAsync(_interviewApiOptions.Value.GetTask, cancellationToken);
        var resultString = await result.Content.ReadAsStringAsync(cancellationToken);
        var response = System.Text.Json.JsonSerializer.Deserialize<GetTaskResponse>(resultString);
        return response ?? new GetTaskResponse();
    }
}