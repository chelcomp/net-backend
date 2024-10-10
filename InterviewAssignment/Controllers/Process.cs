using InterviewAssignment.Business;
using InterviewAssignment.Controllers.Domains;
using InterviewAssignment.Controllers.Maps;
using InterviewAssignment.Database.Repositories.DbOperation;
using InterviewAssignment.Database.Repositories.DbOperation.Models;
using InterviewAssignment.Database.Repositories.DbOperationResult;
using InterviewAssignment.Database.Repositories.DbOperationResult.Models;
using InterviewAssignment.Database.Setup;
using InterviewAssignment.Infrastructure.InterviewApi;
using InterviewAssignment.Infrastructure.InterviewApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssignment.Controllers;

[ApiController]
public class Process : ControllerBase
{
    private readonly IServices _services;
    private readonly IEnqueueDbOperationResultRepository _enqueueDbOperationResultRepository;
    private readonly IEnqueueDbOperationRepository _enqueueDbOperationRepository;
    private readonly IInterviewApi _interviewApi;

    public Process(IServices services,
        IEnqueueDbOperationResultRepository enqueueDbOperationResultRepository,
        IInterviewApi interviewApi,
        IEnqueueDbOperationRepository enqueueDbOperationRepository)
    {
        _services = services;
        _enqueueDbOperationResultRepository = enqueueDbOperationResultRepository;
        _interviewApi = interviewApi;
        _enqueueDbOperationRepository = enqueueDbOperationRepository;
    }

    [HttpGet("api/Process")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var getTaskResponse = await _interviewApi.GetTaskAsync(cancellationToken);
            getTaskResponse.CorrelationId = correlationId;

            var operationModel = getTaskResponse.MapToOperationModel();
            _ = _enqueueDbOperationRepository.CreateAsync(operationModel, cancellationToken);

            return Ok(getTaskResponse);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }

    [HttpPost("api/Process")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(OperationModel modelRequest, CancellationToken cancellationToken)
    {
        var initialCorrelationId = modelRequest.CorrelationId;
        try
        {
            var resultCalculated = _services.Calculate(modelRequest);
            var operationResultView =  modelRequest.MapToOperationResultView(resultCalculated);
            var responseString = await _interviewApi.SubmitTaskAsync(operationResultView, cancellationToken);
            
            var operationResultModel = modelRequest.MapToOperationResultModel( resultCalculated, responseString, initialCorrelationId);
            _ = _enqueueDbOperationResultRepository.CreateAsync(operationResultModel, cancellationToken);

            return Ok(responseString);
        }
        catch
        {
            return StatusCode(500);
        }
    }

   
}