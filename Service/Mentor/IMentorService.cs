using TraineeManagement.Api.DTO.MentorDTO;

namespace TraineeManagement.Api.Service.MentorInterface;

public interface IMentorService
{
    Task<List<MentorResponseModel>> GetMentors();

    Task<MentorResponseModel?> GetMentorById(long id);

    Task<MentorResponseModel> CreateMentor(CreateMentorRequestModel mentor);

    Task<MentorResponseModel?> UpdateMentor(long id, UpdateMentorRequestModel updatedMentor);

    Task<bool> DeleteMentor(long id);
}