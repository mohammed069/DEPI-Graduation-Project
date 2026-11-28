using Microsoft.EntityFrameworkCore;
using MissingPersons.DAL.Models;

namespace MissingPersons.DAL.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly LostFoundContext _context;

    public ReportRepository(LostFoundContext context)
    {
        _context = context;
    }

    public async Task<Report?> GetByIdAsync(int id)
    {
        return await _context.Reports
            .FirstOrDefaultAsync(r => r.Id == id && r.IsDeleted != true);
    }

    public async Task<Report?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Reports
            .Include(r => r.PersonDetail)
            .Include(r => r.ItemDetail)
            .Include(r => r.ReportImages)
            .Include(r => r.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == id && r.IsDeleted != true);
    }

    public async Task<List<Report>> GetByUserIdAsync(int userId)
    {
        return await _context.Reports
            .Where(r => r.UserId == userId && r.IsDeleted != true)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<(List<Report> Items, int TotalCount)> GetPagedAsync(
        string? reportType, string? targetType, string? status,
        string? city, int page, int pageSize)
    {
        var query = _context.Reports
            .Include(r => r.ReportImages)
            .Where(r => r.IsDeleted != true);

        if (!string.IsNullOrWhiteSpace(reportType))
            query = query.Where(r => r.ReportType == reportType);

        if (!string.IsNullOrWhiteSpace(targetType))
            query = query.Where(r => r.TargetType == targetType);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status == status);

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(r => r.City == city);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task AddAsync(Report report)
    {
        await _context.Reports.AddAsync(report);
    }

    public void Update(Report report)
    {
        report.UpdatedAt = DateTime.Now;
        _context.Reports.Update(report);
    }

    public void SoftDelete(Report report)
    {
        report.IsDeleted = true;
        report.UpdatedAt = DateTime.Now;
    }
    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        return await _context.ItemCategories.AnyAsync(c => c.Id == categoryId);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}