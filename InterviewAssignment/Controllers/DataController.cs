using InterviewAssignment.Controllers.Domains;
using InterviewAssignment.Controllers.Maps;
using InterviewAssignment.Database.Repositories.DbOperation;
using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Repositories.DbOperationResult;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssignment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly IDbOperationRepository _dbOperationRepository;
    private readonly IDbOperationResultRepository _dbOperationResultRepository;

    public DataController(IDbOperationRepository dbOperationRepository,
        IDbOperationResultRepository dbOperationResultRepository)
    {
        _dbOperationRepository = dbOperationRepository;
        _dbOperationResultRepository = dbOperationResultRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<OperationView>> Get(CancellationToken cancellationToken)
    {
        var operationModels = await _dbOperationRepository.GetAllAsync(cancellationToken);
        var operationResultResponses = operationModels.MapToOperationView();
        return operationResultResponses;
    }

    

    [HttpGet("operationresult")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<OperationResultView>> GetOperationResult(CancellationToken cancellationToken)
    {
        var operationResultModels =  await _dbOperationResultRepository.GetAllAsync(cancellationToken);
        var operationResultResponses = operationResultModels.MapToOperationView();
        return operationResultResponses;
    }
}