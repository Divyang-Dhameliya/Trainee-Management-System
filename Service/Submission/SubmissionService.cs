using System.Net;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTO.SubmissionDTO;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.SubmissionInterface;

public class SubmissionService : ISubmissionService
{
    private readonly AppDbContext _context;

    public SubmissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SubmissionResponseModel> CreateSubmission(CreateSubmissionRequestModel submission)
    {
        SubmissionModel newSubmission = new SubmissionModel(
            submission.TaskAssignmentId,
            submission.SubmissionUrl,
            submission.Notes,
            submission.SubmittedDate,
            submission.Status
        );

        _context.Submissions.Add(newSubmission);
        await _context.SaveChangesAsync();

        SubmissionResponseModel SubmissionResponseModel = new SubmissionResponseModel(
            newSubmission.Id,
            newSubmission.TaskAssignmentId,
            newSubmission.SubmissionUrl,
            newSubmission.Notes,
            newSubmission.SubmittedDate,
            newSubmission.Status
        );

        return SubmissionResponseModel;
    }

    public async Task<SubmissionResponseModel?> GetSubmissionById(long id)
    {
        SubmissionModel? submission = await _context.Submissions.FindAsync(id);

        if(submission == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "Submission not found with given ID.");
        }
       
        return new SubmissionResponseModel(
            submission.Id,
            submission.TaskAssignmentId,
            submission.SubmissionUrl,
            submission.Notes,
            submission.SubmittedDate,
            submission.Status
        );
    }

    public async Task<List<SubmissionResponseModel>> GetSubmissions()
    {
        List<SubmissionResponseModel> SubmissionResponseModels = new([]);

        List<SubmissionModel> submissions = await _context.Submissions.ToListAsync();

        foreach (SubmissionModel submission in submissions)
        {
            SubmissionResponseModels.Add(
                new SubmissionResponseModel(
                    submission.Id,
                    submission.TaskAssignmentId,
                    submission.SubmissionUrl,
                    submission.Notes,
                    submission.SubmittedDate,
                    submission.Status
                )
            );
        }

        return SubmissionResponseModels;
    }
}