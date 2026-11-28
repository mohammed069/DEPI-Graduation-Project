using Microsoft.Extensions.DependencyInjection;
using MissingPersons.BLL.Services;
using MissingPersons.BLL.Validators;

namespace MissingPersons.BLL;

public static class DependencyInjection {
    public static IServiceCollection AddBLL(this IServiceCollection services) {
        // services.AddScoped<IReportRepository, ReportRepository>();
        // services.AddScoped<IReportService, ReportService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        // Repositories
        services.AddScoped<IReportRepository, ReportRepository>();

        // Services
        services.AddScoped<IReportService, ReportService>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateReportDtoValidator>();

        return services;
    }
}