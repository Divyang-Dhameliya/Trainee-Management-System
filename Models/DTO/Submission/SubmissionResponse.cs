using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.Submission;
using TraineeManagement.Api.Models;

namespace TraineeManagement.Api.DTO.SubmissionDTO;

public class SubmissionResponseModel
{
    public long Id { get; set; }
    
    public long TaskAssignmentId { get; set; }

    public string? SubmissionUrl { get; set; }

    public string? Notes { get; set; }  

    public DateTime? SubmittedDate { get; set; }

    public SubmissionStatusEnum? Status { get; set; }
    
    public SubmissionResponseModel() { }
    
    public SubmissionResponseModel(long Id, long TaskAssignmentId, string? SubmissionUrl, string? Notes, DateTime? SubmittedDate, SubmissionStatusEnum? Status)
    {
        this.Id = Id;
        this.TaskAssignmentId = TaskAssignmentId;
        this.SubmissionUrl  = SubmissionUrl;
        this.Notes = Notes;
        this.SubmittedDate = SubmittedDate;
        this.Status = Status;
    }
}