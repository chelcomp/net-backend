using System.Text.Json.Serialization;

namespace InterviewAssignment.Controllers.Domains;

public record OperationResultView
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("result")] 
    public double Result { get; set; }

    public string Description { get; set; }

    public string CorrelationId { get; set; }
}