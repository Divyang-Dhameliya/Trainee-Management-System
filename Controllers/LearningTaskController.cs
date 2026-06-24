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

    public LearningTaskController(ILearningTaskService learningTaskService)
    {
        _learningTaskService = learningTaskService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        List <LearningTaskResponseModel> LearningTasks = await _learningTaskService.GetLearningTasks();
                        
        return Ok(LearningTasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        LearningTaskResponseModel? LearningTask = await _learningTaskService.GetLearningTaskById(id);

        return Ok(LearningTask);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateLearningTaskRequestModel LearningTask)
    {
        LearningTaskResponseModel newLearningTask = await _learningTaskService.CreateLearningTask(LearningTask);

        return Ok(newLearningTask);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _learningTaskService.DeleteLearningTask(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, UpdateLearningTaskRequestModel updateLearningTaskRequest)
    {
        LearningTaskResponseModel? LearningTask = await _learningTaskService.UpdateLearningTask(id, updateLearningTaskRequest);

        return Ok(LearningTask);
    }
}
