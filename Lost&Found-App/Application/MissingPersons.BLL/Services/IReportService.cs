namespace MissingPersons.BLL.Services;

public interface IReportService
{
    Task<Result<int>> CreateAsync(CreateReportDto dto);
    Task<Result<ReportDto>> GetByIdAsync(int id);
    Task<Result<List<ReportDto>>> GetMyReportsAsync(int userId);
    Task<Result<PagedResult<ReportDto>>> GetPagedAsync(ReportFilterDto filter);
    Task<Result> UpdateAsync(int id, int userId, UpdateReportDto dto);
    Task<Result> DeleteAsync(int id, int userId);
}
