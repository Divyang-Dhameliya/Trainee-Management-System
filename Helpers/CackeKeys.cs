public static class CacheKeys
{
    public static string SubmissionSummary(long id) => $"Submission-Summary:{id}";

    public static string Trainee(long id) => $"Trainee:{id}";

    public static string TaskAssignment(long id) => $"TaskAssignment:{id}";

    public static string TraineesAll => "TraineesAll";
    
    public static string SubmissionsAll => "SubmissionsAll";

    public static string TaskAssignmentsAll => "TaskAssignmentsAll";
}