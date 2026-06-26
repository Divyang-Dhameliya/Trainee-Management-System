using Microsoft.AspNetCore.Mvc;

namespace TrainingDirectory.Api.Controllers;

[ApiController]
[Route("api/trainee-service")]
public class TraineeProfileController : ControllerBase
{

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(new TraineeProfileResponse (
            Id: id,
            Name: "Divyang",
            Email: "dd@gmail.com",
            Role: "Trainee"
        )); 
    }
}
