public record TraineProfileRequest(int Id);

public record TraineeProfileResponse(
    int Id, 
    string Name, 
    string Email, 
    string Role
);
