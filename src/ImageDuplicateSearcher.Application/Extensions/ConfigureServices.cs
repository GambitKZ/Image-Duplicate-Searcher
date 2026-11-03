using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ImageDuplicateSearcher.Application.Common.Settings;
using ImageDuplicateSearcher.Application.Image.Interfaces;
using ImageDuplicateSearcher.Application.Image.Services;
using ImageDuplicateSearcher.Application.Reporting.Interfaces;
using ImageDuplicateSearcher.Application.Reporting.Services;
using ImageDuplicateSearcher.Application.Workflow.Services;

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
