using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.TraineeDTO;
using TraineeManagement.Api.Service.TraineeeInterface;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.Enum.Trainee;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/trainees")]
[Authorize]
public class TraineeController : ControllerBase
{
    private readonly ITraineeService _traineeService;
    private readonly ILogger<TraineeController> _logger;

    public TraineeController(ITraineeService traineeService, ILogger<TraineeController> logger)
    {
        _traineeService = traineeService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber=1, int pageSize=5, string? search=null,TraineeStatus status=TraineeStatus.Active)
    {
        _logger.LogInformation("HTTP GET received for getTrainees. pageNumber: {pageNumber}, pageSize: {pageSize}, searchQuery: {searchQuery}, status: {status}", pageNumber, pageSize, search, status); 

        if (search == null)
        {
            List <TraineeResponseModel> trainees = await _traineeService.GetTrainees();
            _logger.LogInformation("GetTrainees completed successfully.");
            return Ok(trainees);
        }

        PaginationTraineeResponse res = await _traineeService.SearchTrainee(search, status, pageNumber, pageSize);
        _logger.LogInformation("GetTrainees Query completed successfully.");
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getTraineeByID. TraineeId: {Traineeid}", id); 
        TraineeResponseModel? trainee = await _traineeService.GetTraineeById(id);

        if (trainee == null) 
        { 
            _logger.LogWarning("getTraineeByID, Trainee not found with given ID: {TraineeID}", id);
            return NotFound("Trainee not found with given ID"); 
        }

        _logger.LogInformation("GetTraineeByID completed successfully.");
        return Ok(trainee);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTraineeRequestModel trainee)
    {
        _logger.LogInformation("HTTP POST received for CreateTrainee.");
        TraineeResponseModel newtrainee = await _traineeService.CreateTrainee(trainee);
        _logger.LogInformation("HTTP POST CreateTrainee completed successfully.");

        return Ok(newtrainee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        _logger.LogInformation("HTTP DELETE received for TraineeID : {Traineeid}", id);

        bool isDeleted = await _traineeService.DeleteTrainee(id);

        if (!isDeleted) 
        {
            _logger.LogWarning("DeleteByID, Trainee not found with given ID: {TraineeID}", id); 
            return NotFound("Trainee not found with given ID");
        }

        _logger.LogInformation("DeleteByID completed successfully");
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id,UpdateTraineeRequestModel updateTraineeRequest)
    {
        _logger.LogInformation("HTTP PUT received for UpdateTrainee.");

        TraineeResponseModel? trainee = await _traineeService.UpdateTrainee(id, updateTraineeRequest);

        if (trainee == null) 
        { 
            _logger.LogWarning("DeleteByID, Trainee not found with given ID: {TraineeID}", id); 
            return NotFound("Trainee not found with given ID"); 
        }

        _logger.LogInformation("HTTP PUT UpdateTrainee completed successfully.");

        return Ok(trainee);
    }
}
