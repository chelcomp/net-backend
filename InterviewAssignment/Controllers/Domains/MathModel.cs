using System.Text.Json.Serialization;

namespace InterviewAssignment.Controllers.Domains;

public class MathModel
{
    private string _operation;
    public string id { get; set; }

    [JsonPropertyName("operation")]
    public string Operation
    {
        get => _operation;
        set => _operation = value.Trim().ToLower();
    }

    [JsonPropertyName("left")] public double Left { get; set; }

    [JsonPropertyName("right")] public double Right { get; set; }

    public string CorrelationId { get; set; }
}