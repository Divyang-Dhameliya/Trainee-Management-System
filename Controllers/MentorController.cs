using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.Service.MentorInterface;
using TraineeManagement.Api.Enum.Mentor;
using TraineeManagement.Api.DTO.MentorDTO;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/mentors")]
[Authorize]
public class MentorController : ControllerBase
{
    private readonly IMentorService _mentorService;
    private readonly ILogger<MentorController> _logger;

    public MentorController(IMentorService mentorService, ILogger<MentorController> logger)
    {
        _mentorService = mentorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("HTTP GET received for getMentor.");
        
        List <MentorResponseModel> Mentors = await _mentorService.GetMentors();

        _logger.LogInformation("GetMentors completed successfully.");
        
        return Ok(Mentors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getMentorByID. MentorId: {Mentorid}", id); 
        MentorResponseModel? Mentor = await _mentorService.GetMentorById(id);

        if (Mentor == null) 
        { 
            _logger.LogWarning("getMentorByID, Mentor not found with given ID: {MentorID}", id);
            return NotFound("Mentor not found with given ID"); 
        }

        _logger.LogInformation("GetMentorByID completed successfully.");
        return Ok(Mentor);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateMentorRequestModel Mentor)
    {
        _logger.LogInformation("HTTP POST received for CreateMentor.");
        MentorResponseModel newMentor = await _mentorService.CreateMentor(Mentor);
        _logger.LogInformation("HTTP POST CreateMentor completed successfully.");

        return Ok(newMentor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        _logger.LogInformation("HTTP DELETE received for MentorID : {Mentorid}", id);

        bool isDeleted = await _mentorService.DeleteMentor(id);

        if (!isDeleted) 
        {
            _logger.LogWarning("DeleteByID, Mentor not found with given ID: {MentorID}", id); 
            return NotFound("Mentor not found with given ID");
        }

        _logger.LogInformation("DeleteByID completed successfully");
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, UpdateMentorRequestModel updateMentorRequest)
    {
        _logger.LogInformation("HTTP PUT received for UpdateMentor.");

        MentorResponseModel? Mentor = await _mentorService.UpdateMentor(id, updateMentorRequest);

        if (Mentor == null) 
        { 
            _logger.LogWarning("DeleteByID, Mentor not found with given ID: {MentorID}", id); 
            return NotFound("Mentor not found with given ID"); 
        }

        _logger.LogInformation("HTTP PUT UpdateMentor completed successfully.");

        return Ok(Mentor);
    }
}
