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
    public List<TraineeResponse> Get()
    {
        var trainees = _traineeService.GetTrainees();
        return trainees;
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] long id)
    {
        TraineeResponse trainee = _traineeService.GetTraineeById(id);

        if(trainee == null){ return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }

    [HttpPost]
    public IActionResult Post([FromBody] CreateTraineeRequest trainee)
    {
        TraineeResponse newtrainee = _traineeService.CreateTrainee(trainee);

        return Ok(newtrainee);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        bool isDeleted = _traineeService.DeleteTrainee(id);

        if(!isDeleted) return NotFound("Trainee not found with given ID");

        return NoContent();
    }  

    [HttpPut]
    public IActionResult Put(UpdateTraineeRequest updateTraineeRequest)
    {
        TraineeResponse trainee = _traineeService.UpdateTrainee(updateTraineeRequest);

        if(trainee == null){ return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }
}
