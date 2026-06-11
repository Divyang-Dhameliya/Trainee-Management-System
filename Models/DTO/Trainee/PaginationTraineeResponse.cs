namespace TraineeManagement.Api.DTO.TraineeDTO;

public class PaginationTraineeResponse
{
    public int pageNumber { get; set; }

    public int pageSize { get; set; }

    public int totalRecords { get; set; }

    public IEnumerable<TraineeResponseModel> data { get; set; }

    public PaginationTraineeResponse(int pageNumber, int pageSize, int totalRecords, IEnumerable<TraineeResponseModel> data)
    {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.totalRecords = totalRecords;
        this.data = data;
    }
}