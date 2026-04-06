using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Services;
using ImageDuplicateSearcher.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageDuplicateSearcher.Application.Extensions;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ImageDuplicationOptions>(configuration.GetSection(ImageDuplicationOptions.Section));

        services.AddTransient<WorkflowService>();
        services.AddTransient<IImageProcessor, ImageProcessor>();
        services.AddTransient<IReporter, Reporter>();
    }

}
