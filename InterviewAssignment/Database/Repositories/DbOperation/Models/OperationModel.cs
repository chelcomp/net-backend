using System.Text.Json.Serialization;

namespace InterviewAssignment.Database.Repositories.DbOperation.Models;

public class OperationModel
{
    public string Id { get; set; }

    public string Operation { get; set; }

    public double Left { get; set; }

    public double Right { get; set; }

    public string CorrelationId { get; set; }
}