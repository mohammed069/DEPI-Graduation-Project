using Microsoft.Extensions.DependencyInjection;

namespace MissingPersons.BLL;

public static class DependencyInjection {
    public static IServiceCollection AddBLL(this IServiceCollection services) {
        // services.AddScoped<IReportRepository, ReportRepository>();
        // services.AddScoped<IReportService, ReportService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}