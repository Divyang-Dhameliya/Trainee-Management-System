using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.TraineeDTO;
using TraineeManagement.Api.Service.TraineeeInterface;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.Enum.Trainee;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/trainees")]
[Authorize]
public class TraineeController : ControllerBase
{
    private readonly ITraineeService _traineeService;
    private readonly TraineeProfileClient _traineeProfileService;

    public TraineeController(ITraineeService traineeService, TraineeProfileClient traineeProfileService)
    {
        _traineeService = traineeService;
        _traineeProfileService = traineeProfileService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber=1, int pageSize=5, string? search=null,TraineeStatus status=TraineeStatus.Active)
    {

        if (search == null)
        {
            List <TraineeResponseModel> trainees = await _traineeService.GetTrainees();
            return Ok(trainees);
        }

        PaginationTraineeResponse res = await _traineeService.SearchTrainee(search, status, pageNumber, pageSize);
        
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        TraineeResponseModel? trainee = await _traineeService.GetTraineeById(id);

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
        await _traineeService.DeleteTrainee(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id,UpdateTraineeRequestModel updateTraineeRequest)
    {
        TraineeResponseModel? trainee = await _traineeService.UpdateTrainee(id, updateTraineeRequest);

        return Ok(trainee);
    }

    [HttpGet("{id}/dispatch")] // InterService Communication
    public async Task<IActionResult> DispatchTraineeRequest(int id , CancellationToken cancellationToken)
    {

        TraineeProfileResponse? result = await _traineeProfileService.FetchTraineProfileById(id, cancellationToken);
        return Ok(result);
    }
}
