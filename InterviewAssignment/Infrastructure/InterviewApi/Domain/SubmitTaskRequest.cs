using System.Text.Json.Serialization;

namespace InterviewAssignment.Infrastructure.InterviewApi.Domain;

public record SubmitTaskRequest
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("result")] 
    public double Result { get; set; }
}