using System.Text.Json.Serialization;

namespace InterviewAssignment.Infrastructure.InterviewApi.Domain;

public class GetTaskResponse
{
    public string Id { get; set; }

    [JsonPropertyName("operation")]
    public string Operation { get; set; }

    [JsonPropertyName("left")]
    public double Left { get; set; }

    [JsonPropertyName("right")]
    public double Right { get; set; }

    public string CorrelationId { get; set; }
}