using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Setup;

namespace InterviewAssignment.Business;

public interface IServices
{
    double Calculate(OperationModel modelRequest);
}