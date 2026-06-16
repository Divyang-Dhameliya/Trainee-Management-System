namespace TraineeManagement.Api.Constants;

public static class TaskAssignmentConstants 
{
    public const string TraineeIdRequiredErrorMessage = "TraineeId is required";
    public const string MentorIdRequiredErrorMessage = "MentorId is required";
    public const string LearningTaskIdRequiredErrorMessage = "LearningTaskId is required";
    public const string DueDateRequiredErrorMessage = "DueDate is required";
    public const string AssignedDateRequiredErrorMessage = "AssignedDate is required";
    public const string StatusRequiredErrorMessage = "Status is required";
    public const string StatusValidateErrorMessage = "TaskAssignment status can be either Assigned / InProgress / Submitted / Reviewed / Completed";
}