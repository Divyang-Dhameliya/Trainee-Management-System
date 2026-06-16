using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.DTO.TaskAssignmentDTO;
using TraineeManagement.Api.Service.TaskAssignmentInterface;
using TraineeManagement.Api.Enum.TaskAssignment;
using ZstdSharp;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/task-assignments")]
[Authorize]
public class TaskAssignmentController : ControllerBase
{
    private readonly ITaskAssignmentService _taskAssignmentService;
    private readonly ILogger<TaskAssignmentController> _logger;

    public TaskAssignmentController(ITaskAssignmentService taskAssignmentService, ILogger<TaskAssignmentController> logger)
    {
        _taskAssignmentService = taskAssignmentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("HTTP GET received for getTaskAssignment.");
        
        List <TaskAssignmentResponseModel> TaskAssignments = await _taskAssignmentService.GetTaskAssignments();

        _logger.LogInformation("GetTaskAssignments completed successfully.");
        
        return Ok(TaskAssignments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getTaskAssignmentByID. TaskAssignmentId: {TaskAssignmentid}", id); 
        TaskAssignmentResponseModel? TaskAssignment = await _taskAssignmentService.GetTaskAssignmentById(id);
        
        _logger.LogInformation("GetTaskAssignmentByID completed successfully.");
        return Ok(TaskAssignment);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTaskAssignmentRequestModel TaskAssignment)
    {
        _logger.LogInformation("HTTP POST received for CreateTaskAssignment.");

        try
        {
           TaskAssignmentResponseModel newTaskAssignment = await _taskAssignmentService.CreateTaskAssignment(TaskAssignment);
           _logger.LogInformation("HTTP POST CreateTaskAssignment completed successfully.");
           return Ok(newTaskAssignment); 
        }
        catch(Exception ex)
        {
            _logger.LogError("POST CreateTaskAssignment Failed with Error: {message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> Put(long id,[FromBody] StatusUpdateRequest request)
    {
        _logger.LogInformation("HTTP PUT received for UpdateTaskAssignment status: {status}", request.Status);
        
        TaskAssignmentResponseModel? TaskAssignment = await _taskAssignmentService.UpdateTaskAssignment(id, request.Status);

        _logger.LogInformation("HTTP PUT UpdateTaskAssignment completed successfully.");

        return Ok(TaskAssignment);
    }
}
