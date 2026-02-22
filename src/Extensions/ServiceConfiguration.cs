using ImageDuplicateSearcher.Interfaces;
using ImageDuplicateSearcher.Services;
using ImageDuplicateSearcher.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageDuplicateSearcher.Extensions;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ImageDuplicationOptions>(configuration.GetSection(ImageDuplicationOptions.Section));

        services.AddTransient<WorkflowService>();
        services.AddTransient<IImageProcessor, ImageProcessor>();
        services.AddTransient<IDuplicateGrouper, DuplicateGrouper>();
        services.AddTransient<IReporter, Reporter>();
    }

}
