using System.Net;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTO.TraineeDTO;

using TraineeManagement.Api.Service.TraineeeInterface;
using Microsoft.AspNetCore.Authorization;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/trainees")]
[Authorize]
public class TraineeController : ControllerBase
{
    private readonly ITraineeService _traineeService;

    public TraineeController(ITraineeService traineeService)
    {
        _traineeService = traineeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string? search = null)
    {

        if (search == null)
        {
            return Ok(await _traineeService.GetTrainees());
        }

        return Ok(await _traineeService.SearchTrainee(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        TraineeResponseModel? trainee = await _traineeService.GetTraineeById(id);

        if (trainee == null) { return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTraineeRequestModel trainee)
    {
        TraineeResponseModel newtrainee = await _traineeService.CreateTrainee(trainee);

        return Ok(newtrainee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        bool isDeleted = await _traineeService.DeleteTrainee(id);

        if (!isDeleted) return NotFound("Trainee not found with given ID");

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateTraineeRequestModel updateTraineeRequest)
    {
        TraineeResponseModel? trainee = await _traineeService.UpdateTrainee(updateTraineeRequest);

        if (trainee == null) { return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }
}
