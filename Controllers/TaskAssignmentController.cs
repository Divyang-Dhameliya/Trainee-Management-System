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

    public TaskAssignmentController(ITaskAssignmentService taskAssignmentService)
    {
        _taskAssignmentService = taskAssignmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {        
        List <TaskAssignmentResponseModel> TaskAssignments = await _taskAssignmentService.GetTaskAssignments();
        
        return Ok(TaskAssignments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        TaskAssignmentResponseModel? TaskAssignment = await _taskAssignmentService.GetTaskAssignmentById(id);
        
        return Ok(TaskAssignment);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTaskAssignmentRequestModel TaskAssignment)
    {
        TaskAssignmentResponseModel newTaskAssignment = await _taskAssignmentService.CreateTaskAssignment(TaskAssignment);
        
        return Ok(newTaskAssignment); 
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> Put(long id,[FromBody] StatusUpdateRequest request)
    {
        
        TaskAssignmentResponseModel? TaskAssignment = await _taskAssignmentService.UpdateTaskAssignment(id, request.Status);

        return Ok(TaskAssignment);
    }
}
