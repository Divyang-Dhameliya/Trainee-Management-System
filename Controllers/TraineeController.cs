using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Models;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/trainees")]
public class TraineeController : ControllerBase
{
    private static readonly List<Trainee> trainees = new List<Trainee> (
    [
        new Trainee(
            1,
            "Divyang",
            "Dhameliya",
            "dd@zeuslearning.com",
            "HTML,CSS,JS",
            "Active"
        ),
        new Trainee(
            2,
            "Khanjan",
            "Fadadu",
            "kf@zeuslearning.com",
            "HTML,CSS,.NET",
            "InActive"
        )
    ]);

    [HttpGet]
    public List<Trainee> Get()
    {
        return trainees;
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] long id)
    {
        Trainee trainee = trainees.FirstOrDefault(t => t.Id == id);

        if(trainee == null){ return NotFound("Trainee not found with given ID"); }

        return Ok(trainee);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Trainee trainee)
    {
        trainee.CreatedDate = DateTime.UtcNow;
        trainee.UpdatedDate = DateTime.UtcNow;
        trainee.Id = trainees.Count + 1;
        trainees.Add(trainee);

        return Ok(trainee);
    }
}
