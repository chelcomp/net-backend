namespace InterviewAssignment.Infrastructure.InterviewApi;

public record InterviewApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string SubmitTask { get; set; } = string.Empty;
    public string GetTask { get; set; } = string.Empty;
}