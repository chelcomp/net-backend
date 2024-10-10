using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Setup;

namespace InterviewAssignment.Business;

public class Services : IServices
{
    public double Calculate(OperationModel modelRequest)
    {
        var resultMath = modelRequest.Operation switch
        {
            "division" => modelRequest.Left / modelRequest.Right,
            "multiplication" => modelRequest.Left * modelRequest.Right,
            "addition" => modelRequest.Left + modelRequest.Right,
            "subtraction" => modelRequest.Left - modelRequest.Right,
            "remainder" => modelRequest.Left % modelRequest.Right,
            _ => 0
        };

        return resultMath;
    }
}