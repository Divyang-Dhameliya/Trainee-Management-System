using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.Service.MentorInterface;
using TraineeManagement.Api.Enum.Mentor;
using TraineeManagement.Api.DTO.MentorDTO;
using StackExchange.Redis;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/mentors")]
public class MentorController : ControllerBase
{
    private readonly IMentorService _mentorService;

    public MentorController(IMentorService mentorService)
    {
        _mentorService = mentorService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get()
    {        
        List <MentorResponseModel> Mentors = await _mentorService.GetMentors();
        
        return Ok(Mentors);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        MentorResponseModel? Mentor = await _mentorService.GetMentorById(id);

        return Ok(Mentor);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Mentor")]
    public async Task<IActionResult> Post([FromBody] CreateMentorRequestModel Mentor)
    {
        MentorResponseModel newMentor = await _mentorService.CreateMentor(Mentor);

        return Ok(newMentor);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Mentor")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mentorService.DeleteMentor(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Mentor")]
    public async Task<IActionResult> Put(long id, UpdateMentorRequestModel updateMentorRequest)
    {
        MentorResponseModel? Mentor = await _mentorService.UpdateMentor(id, updateMentorRequest);

        return Ok(Mentor);
    }
}
