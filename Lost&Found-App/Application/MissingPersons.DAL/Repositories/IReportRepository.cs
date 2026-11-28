using MissingPersons.DAL.Models;

namespace MissingPersons.DAL.Repositories;

public interface IReportRepository
{
    Task<Report?> GetByIdAsync(int id);
    Task<Report?> GetByIdWithDetailsAsync(int id);
    Task<List<Report>> GetByUserIdAsync(int userId);
    Task<(List<Report> Items, int TotalCount)> GetPagedAsync(
        string? reportType, string? targetType, string? status,
        string? city, int page, int pageSize);

    Task AddAsync(Report report);
    void Update(Report report);
    void SoftDelete(Report report);
    Task<int> SaveChangesAsync();
    Task<bool> CategoryExistsAsync(int categoryId);
}
