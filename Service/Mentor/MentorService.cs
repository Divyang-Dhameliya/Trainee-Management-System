using TraineeManagement.Api.Models;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Service.MentorInterface;
using TraineeManagement.Api.DTO.MentorDTO;

namespace TraineeManagement.Api.Service.MentorService;

public class MentorService : IMentorService
{
    private readonly AppDbContext _context;

    public MentorService(AppDbContext context)
    { 
        _context = context; 
    }
    
    public async Task<List<MentorResponseModel>> GetMentors()
    {
        List<MentorResponseModel> MentorResponseModels = new([]);

        List<MentorModel> mentors = await _context.Mentors.ToListAsync();

        foreach (MentorModel mentor in mentors)
        {
            MentorResponseModels.Add(
                new MentorResponseModel(
                    mentor.Id,
                    mentor.FirstName,
                    mentor.LastName,
                    mentor.Email,
                    mentor.Expertise,
                    mentor.Status,
                    mentor.CreatedDate,
                    mentor.UpdatedDate
                )
            );
        }

        return MentorResponseModels;
    }

    public async Task<MentorResponseModel?> GetMentorById(long id)
    {
        MentorModel? mentor = await _context.Mentors.FindAsync(id);

        if (mentor != null)
        {
            return new MentorResponseModel(
                mentor.Id,
                mentor.FirstName,
                mentor.LastName,
                mentor.Email,
                mentor.Expertise,
                mentor.Status,
                mentor.CreatedDate,
                mentor.UpdatedDate
            );
        }

        return null;
    }
    public async Task<MentorResponseModel> CreateMentor(CreateMentorRequestModel mentor)
    {
        MentorModel newMentor = new MentorModel(
            mentor.FirstName,
            mentor.LastName,
            mentor.Email,
            mentor.Expertise,
            mentor.Status
        );

        _context.Mentors.Add(newMentor);
        await _context.SaveChangesAsync();

        MentorResponseModel MentorResponseModel = new MentorResponseModel(
            newMentor.Id,
            newMentor.FirstName,
            newMentor.LastName,
            newMentor.Email,
            newMentor.Expertise,
            newMentor.Status,
            newMentor.CreatedDate,
            newMentor.UpdatedDate
        );

        return MentorResponseModel;
    }

    public async  Task<MentorResponseModel?> UpdateMentor(long id, UpdateMentorRequestModel updatedMentor)
    {
        MentorModel? mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null)
        {
            return null;
        }

        mentor.FirstName = updatedMentor.FirstName;
        mentor.LastName = updatedMentor.LastName;
        mentor.Email = updatedMentor.Email;
        mentor.Expertise = updatedMentor.Expertise;
        mentor.Status = updatedMentor.Status;
        mentor.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new MentorResponseModel(
            mentor.Id,
            mentor.FirstName,
            mentor.LastName,
            mentor.Email,
            mentor.Expertise,
            mentor.Status,
            mentor.CreatedDate,
            mentor.UpdatedDate
        );
    }

    public async Task<bool> DeleteMentor(long id)
    {
        MentorModel? mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null)
        {
            return false;
        }

        _context.Mentors.Remove(mentor);

        await _context.SaveChangesAsync();

        return true;
    }
}