using System.Net;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTO.TraineeDTO;

using TraineeManagement.Api.Service.TraineeeInterface;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/trainees")]
public class TraineeController : ControllerBase
{
    private readonly ITraineeService _traineeService;

    public TraineeController(ITraineeService traineeService)
    {
        _traineeService = traineeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string search)
    {
        var trainees = await _traineeService.SearchTrainee(search);

        if(trainees == null || trainees.Count() == 0){ return NotFound("Trainee not found with given search query"); }

        return Ok(trainees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        var trainee = await _traineeService.GetTraineeById(id);

        if(trainee == null){ return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTraineeRequest trainee)
    {
        TraineeResponse newtrainee = await _traineeService.CreateTrainee(trainee);

        return Ok(newtrainee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        bool isDeleted = await _traineeService.DeleteTrainee(id);

        if(!isDeleted) return NotFound("Trainee not found with given ID");

        return NoContent();
    }  

    [HttpPut]
    public async Task<IActionResult> Put(UpdateTraineeRequest updateTraineeRequest)
    {
        TraineeResponse trainee =await _traineeService.UpdateTrainee(updateTraineeRequest);

        if(trainee == null){ return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }
}
