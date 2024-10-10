namespace InterviewAssignment.Database.Repositories.DbOperationResult.Models;

public record OperationResultModel
{
    public string Id { get; set; }

    public double Result { get; set; }

    public string Description { get; set; }

    public string CorrelationId { get; set; }
}