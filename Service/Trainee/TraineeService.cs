using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Enum.Trainee;
using System.Net;
using System.Data;
using Dapper;

namespace TraineeManagement.Api.Service.TraineeService;

public class TraineeService : ITraineeService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<TraineeService> _logger;

    public TraineeService(AppDbContext context, ICacheService cacheService, ILogger<TraineeService> logger)
    { 
        _context = context; 
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<List<TraineeResponseModel>> GetTrainees()
    {
        string cacheKey = CacheKeys.TraineesAll;

        List<TraineeResponseModel>? cachedTrainees = await _cacheService.GetAsync<List<TraineeResponseModel>>(cacheKey);
        
        if(cachedTrainees != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedTrainees;
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        List<TraineeResponseModel> TraineeResponseModels = new([]);

        List<TraineeModel> trainees = await _context.Trainees.ToListAsync();


        foreach (TraineeModel trainee in trainees)
        {
            TraineeResponseModels.Add(
                new TraineeResponseModel(
                    trainee.Id,
                    trainee.FirstName,
                    trainee.LastName,
                    trainee.Email,
                    trainee.TechStack,
                    trainee.Status,
                    trainee.CreatedDate,
                    trainee.UpdatedDate
                )
            );
        }

        await _cacheService.SetAsync(cacheKey, TraineeResponseModels, TimeSpan.FromMinutes(10));

        return TraineeResponseModels;
    }

    public async Task<PaginationTraineeResponse> SearchTrainee(string search, TraineeStatus status, int pageNumber, int pageSize)
    {
        // Approach - 1 (Using LINQ & Skip() & Take() Methods)
        
        // List<TraineeResponseModel> trainees = await _context.Trainees.Where(
        //     trainee =>
        //         (trainee.FirstName != null && trainee.FirstName.Contains(search) ||
        //         trainee.LastName !=null && trainee.LastName.Contains(search) ||
        //         trainee.Email != null && trainee.Email.Contains(search) ||
        //         trainee.TechStack != null && trainee.TechStack.Contains(search)) &&
        //         trainee.Status == status
        // ).Select(
        //     trainee => new TraineeResponseModel(
        //         trainee.Id,
        //         trainee.FirstName,
        //         trainee.LastName,
        //         trainee.Email,
        //         trainee.TechStack,
        //         trainee.Status,
        //         trainee.CreatedDate,
        //         trainee.UpdatedDate
        //     )
        // ).ToListAsync();


        // IEnumerable<TraineeResponseModel> traineeRes = trainees.Skip(pageSize*(pageNumber-1)).Take(pageSize);

        // return new PaginationTraineeResponse(pageNumber, pageSize, trainees.Count, traineeRes);

        
        // Approach - 2 (Using Stored Procedure)
        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var parameters = new {
            p_Status = status,
            p_SearchValue = search,
            p_PageNumber = pageNumber,
            p_PageSize = pageSize
        };

        var dbResults = await connection.QueryAsync(
            "GetTraineesPaginatedAndSearched",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        
        int totalEntries = Convert.ToInt32(dbResults.FirstOrDefault()?.TotalCount) ?? 0;

        IEnumerable<TraineeResponseModel> trainees = dbResults.Select(trainee => new TraineeResponseModel (
            (long)trainee.Id,
            (string)trainee.FirstName,
            (string)trainee.LastName,
            (string)trainee.Email,
            (string)trainee.TechStack,
            trainee.Status != null ? (TraineeStatus?)(int)trainee.Status : null,
            (DateTime)trainee.CreatedDate,
            (DateTime)trainee.UpdatedDate
        ));

        return new PaginationTraineeResponse(pageNumber, pageSize, totalEntries, trainees);
    }

    public async Task<TraineeResponseModel?> GetTraineeById(long id)
    {
        string cacheKey = CacheKeys.Trainee(id);

        TraineeResponseModel? cachedTrainee = await _cacheService.GetAsync<TraineeResponseModel>(cacheKey);
        
        if(cachedTrainee != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedTrainee;
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        TraineeModel? trainee = await _context.Trainees.FindAsync(id);
        
        if(trainee == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "Trainee not found with given ID.");
        }


        TraineeResponseModel response =  new TraineeResponseModel(
            trainee.Id,
            trainee.FirstName,
            trainee.LastName,
            trainee.Email,
            trainee.TechStack,
            trainee.Status,
            trainee.CreatedDate,
            trainee.UpdatedDate
        );

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
    }

    public async Task<TraineeResponseModel> CreateTrainee(CreateTraineeRequestModel trainee)
    {
        TraineeModel newTrainee = new TraineeModel(
            trainee.FirstName,
            trainee.LastName,
            trainee.Email,
            trainee.TechStack,
            trainee.Status
        );

        _context.Trainees.Add(newTrainee);
        await _context.SaveChangesAsync();

        await _cacheService.RemoveAsync(CacheKeys.TraineesAll);

        TraineeResponseModel TraineeResponseModel = new TraineeResponseModel(
            newTrainee.Id,
            newTrainee.FirstName,
            newTrainee.LastName,
            newTrainee.Email,
            newTrainee.TechStack,
            newTrainee.Status,
            newTrainee.CreatedDate,
            newTrainee.UpdatedDate
        );

        return TraineeResponseModel;
    }

    public async Task<TraineeResponseModel?> UpdateTrainee(long id, UpdateTraineeRequestModel updatedtrainee)
    {
        TraineeModel? trainee = await _context.Trainees.FindAsync(id);

        if (trainee == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "Trainee not found with given ID.");
        }

        trainee.FirstName = updatedtrainee.FirstName;
        trainee.LastName = updatedtrainee.LastName;
        trainee.Email = updatedtrainee.Email;
        trainee.TechStack = updatedtrainee.TechStack;
        trainee.Status = updatedtrainee.Status;
        trainee.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _cacheService.RemoveAsync(CacheKeys.Trainee(id));
        await _cacheService.RemoveAsync(CacheKeys.TraineesAll);

        return new TraineeResponseModel(
            trainee.Id,
            trainee.FirstName,
            trainee.LastName,
            trainee.Email,
            trainee.TechStack,
            trainee.Status,
            trainee.CreatedDate,
            trainee.UpdatedDate
        );
    }

    public async Task<bool> DeleteTrainee(long id)
    {
        TraineeModel? trainee = await _context.Trainees.FindAsync(id);

        if (trainee == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "Trainee not found with given ID.");
        }

        _context.Trainees.Remove(trainee);

        await _context.SaveChangesAsync();
        
        await _cacheService.RemoveAsync(CacheKeys.Trainee(id));
        await _cacheService.RemoveAsync(CacheKeys.TraineesAll);

        return true;
    }
}