using System.Net;
using Polly.CircuitBreaker;
using Polly.Timeout;

public class TraineeProfileClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TraineeProfileClient> _logger;
    public TraineeProfileClient(HttpClient httpClient, ILogger<TraineeProfileClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TraineeProfileResponse?> FetchTraineProfileById(int id, CancellationToken cancellationtoken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/trainee-service/{id}");
        
        Guid correlationId = Guid.NewGuid();

        request.Headers.Add("X-Correlation-ID", Convert.ToString(correlationId));

        try
        {
            var response = await _httpClient.SendAsync(request, cancellationtoken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpStatusException(HttpStatusCode.BadGateway, "Bad Gateway upstream communication failure.");
            }

            return await response.Content.ReadFromJsonAsync<TraineeProfileResponse>(cancellationtoken);
        }
        catch (Exception ex) when (ex is TimeoutRejectedException or BrokenCircuitException or HttpRequestException)
        {
            // Fallback when downstream is down
            _logger.LogWarning(ex, "Service unavailable. Using fallback.");

            return new TraineeProfileResponse(
                Id: id,
                Name: "Divyang (Fallback)",
                Email: "dd@gmail.com",
                Role: "Trainee"
            );
        }
    }
}