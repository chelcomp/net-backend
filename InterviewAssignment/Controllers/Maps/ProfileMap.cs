using InterviewAssignment.Controllers.Domains;
using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;
using InterviewAssignment.Infrastructure.InterviewApi.Domain;

namespace InterviewAssignment.Controllers.Maps;

public static class ProfileMap
{
    public static IEnumerable<OperationView> MapToOperationView(this IEnumerable<OperationModel> operationModels)
    {
        var operationResultResponses = operationModels.Select(s => new OperationView()
        {
            id = s.Id,
            Left = s.Left,
            Right = s.Right,
            Operation = s.Operation,
            CorrelationId = s.CorrelationId,
        });
        return operationResultResponses;
    }
    public static IEnumerable<OperationResultView> MapToOperationView(this IEnumerable<OperationResultModel> operationResultModels)
    {
        var operationResultResponses = operationResultModels.Select(s => new OperationResultView()
        {
            Id = s.Id,
            Description = s.Description,
            CorrelationId = s.CorrelationId,
            Result = s.Result
        });
        return operationResultResponses;
    }
    
    public static OperationModel MapToOperationModel(this GetTaskResponse getTaskResponse)
    {
        var operationModel = new OperationModel()
        {
            Id = getTaskResponse.Id,
            CorrelationId = getTaskResponse.CorrelationId,
            Left = getTaskResponse.Left,
            Right = getTaskResponse.Right,
            Operation = getTaskResponse.Operation
        };
        return operationModel;
    }
    
    public static SubmitTaskRequest MapToOperationResultView(this OperationModel operationModel, double resultCalculated)
    {
        var operationResultView = new SubmitTaskRequest
        {
            Id = operationModel.Id,
            Result = resultCalculated
        };
        return operationResultView;
    }

    public static OperationResultModel MapToOperationResultModel(this OperationModel operationModel,
        double resultCalculated,
        string responseString,
        string initialCorrelationId)
    {
        var operationResultModel = new OperationResultModel()
        {
            Id = operationModel.Id,
            Result = resultCalculated,
            Description = responseString,
            CorrelationId = string.IsNullOrWhiteSpace(initialCorrelationId)
                ? Guid.NewGuid().ToString()
                : initialCorrelationId
        };
        return operationResultModel;
    }
}