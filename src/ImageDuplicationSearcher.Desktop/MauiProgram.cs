using Microsoft.Extensions.Logging;
using ImageDuplicateSearcher.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ImageDuplicationSearcher.Desktop;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Determine appsettings.json paths (local and Console project fallback)
        var localPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        var consolePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ImageDuplicateSearcher.Console", "appsettings.json");

        // Try to load ImageDuplicationOptions from JSON and register as configured options.
        ImageDuplicateSearcher.Application.Settings.ImageDuplicationOptions parsedOptions = new ImageDuplicateSearcher.Application.Settings.ImageDuplicationOptions();
        var configPath = File.Exists(localPath) ? localPath : (File.Exists(consolePath) ? consolePath : null);
        if (configPath != null)
        {
            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(configPath));
                if (doc.RootElement.TryGetProperty(ImageDuplicateSearcher.Application.Settings.ImageDuplicationOptions.Section, out var section))
                {
                    parsedOptions = System.Text.Json.JsonSerializer.Deserialize<ImageDuplicateSearcher.Application.Settings.ImageDuplicationOptions>(section.GetRawText()) ?? parsedOptions;
                }
            }
            catch
            {
                // ignore parse errors and continue with defaults
            }
        }

        builder.UseMauiApp<App>();

        // Register application services (WorkflowService, ImageProcessor, Reporter) using the same lifetimes as the Console app.
        builder.Services.Configure<ImageDuplicateSearcher.Application.Settings.ImageDuplicationOptions>(opts =>
        {
            opts.ImageDirectory = parsedOptions.ImageDirectory;
            opts.OutputFilePath = parsedOptions.OutputFilePath;
            opts.SupportedFormats = parsedOptions.SupportedFormats;
        });

        builder.Services.AddTransient<ImageDuplicateSearcher.Application.Services.WorkflowService>();
        builder.Services.AddTransient<ImageDuplicateSearcher.Application.Interfaces.IImageProcessor, ImageDuplicateSearcher.Application.Services.ImageProcessor>();
        builder.Services.AddTransient<ImageDuplicateSearcher.Application.Interfaces.IReporter, ImageDuplicateSearcher.Application.Services.Reporter>();
        // Override IReporter for Desktop with UiReporter
        builder.Services.AddTransient<ImageDuplicateSearcher.Application.Interfaces.IReporter, ImageDuplicateSearcher.Application.Services.UiReporter>();

        builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Ensure the application's services are available to the MAUI host
        return builder.Build();
    }
}
