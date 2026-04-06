using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using ImageDuplicationSearcher.Desktop;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Services;
using ImageDuplicateSearcher.Application.Settings;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        // Register the FolderPicker as a singleton.
        builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);

        // Register runtime options used by desktop UI and application services.
        builder.Services.Configure<ImageDuplicationOptions>(options =>
        {
            options.ImageDirectory = string.Empty;
            options.OutputFilePath = Path.Combine(FileSystem.AppDataDirectory, "duplicates.json");
            options.SupportedFormats = new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };
        });

        builder.Services.AddTransient<IImageProcessor, ImageProcessor>();
        builder.Services.AddSingleton<UiReporter>();
        builder.Services.AddTransient<IReporter>(sp => sp.GetRequiredService<UiReporter>());
        builder.Services.AddTransient<WorkflowService>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AppShell>();

        return builder.Build();
    }
}
