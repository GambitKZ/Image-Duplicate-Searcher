using CommunityToolkit.Maui;
using ImageDuplicationSearcher.Desktop;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Services;
using ImageDuplicateSearcher.Application.Settings;
using ImageDuplicationSearcher.Desktop.Services;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        // Register runtime options used by desktop UI and application services.
        builder.Services.Configure<ImageDuplicationOptions>(options =>
        {
            options.ImageDirectory = string.Empty;
            options.OutputFilePath = Path.Combine(FileSystem.AppDataDirectory, "duplicates.json");
            options.SupportedFormats = new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };
        });
        builder.Services.AddSingleton<IResultsLoader, ResultsLoader>();
        builder.Services.AddSingleton<IDuplicateNavigator, DuplicateNavigator>();
        builder.Services.AddSingleton<IImageDisplayManager, ImageDisplayManager>();
        builder.Services.AddSingleton<IImageRemovalService, ImageRemovalService>();

    #if ANDROID
        builder.Services.AddTransient<IPlatformFileService, AndroidPlatformFileService>();
    #else
        builder.Services.AddTransient<IPlatformFileService, WindowsPlatformFileService>();
    #endif
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AppShell>();

        return builder.Build();
    }
}
