using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.Service.LearningTaskInterface;
using TraineeManagement.Api.DTO.LearningTaskDTO;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/learning-tasks")]
[Authorize]
public class LearningTaskController : ControllerBase
{
    private readonly ILearningTaskService _learningTaskService;
    private readonly ILogger<LearningTaskController> _logger;

    public LearningTaskController(ILearningTaskService learningTaskService, ILogger<LearningTaskController> logger)
    {
        _learningTaskService = learningTaskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("HTTP GET received for getLearningTasks."); 

        List <LearningTaskResponseModel> LearningTasks = await _learningTaskService.GetLearningTasks();
            
        _logger.LogInformation("GetLearningTasks completed successfully.");
            
        return Ok(LearningTasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getLearningTaskByID. LearningTaskId: {LearningTaskid}", id); 
        LearningTaskResponseModel? LearningTask = await _learningTaskService.GetLearningTaskById(id);

        if (LearningTask == null) 
        { 
            _logger.LogWarning("getLearningTaskByID, LearningTask not found with given ID: {LearningTaskID}", id);
            return NotFound("LearningTask not found with given ID"); 
        }

        _logger.LogInformation("GetLearningTaskByID completed successfully.");
        return Ok(LearningTask);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateLearningTaskRequestModel LearningTask)
    {
        _logger.LogInformation("HTTP POST received for CreateLearningTask.");
        LearningTaskResponseModel newLearningTask = await _learningTaskService.CreateLearningTask(LearningTask);
        _logger.LogInformation("HTTP POST CreateLearningTask completed successfully.");

        return Ok(newLearningTask);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        _logger.LogInformation("HTTP DELETE received for LearningTaskID : {LearningTaskid}", id);

        bool isDeleted = await _learningTaskService.DeleteLearningTask(id);

        if (!isDeleted) 
        {
            _logger.LogWarning("DeleteByID, LearningTask not found with given ID: {LearningTaskID}", id); 
            return NotFound("LearningTask not found with given ID");
        }

        _logger.LogInformation("DeleteByID completed successfully");
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, UpdateLearningTaskRequestModel updateLearningTaskRequest)
    {
        _logger.LogInformation("HTTP PUT received for UpdateLearningTask.");

        LearningTaskResponseModel? LearningTask = await _learningTaskService.UpdateLearningTask(id, updateLearningTaskRequest);

        if (LearningTask == null) 
        { 
            _logger.LogWarning("DeleteByID, LearningTask not found with given ID: {LearningTaskID}", id); 
            return NotFound("LearningTask not found with given ID"); 
        }

        _logger.LogInformation("HTTP PUT UpdateLearningTask completed successfully.");

        return Ok(LearningTask);
    }
}
