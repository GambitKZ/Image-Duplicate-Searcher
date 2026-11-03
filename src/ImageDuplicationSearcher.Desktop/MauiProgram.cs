using CommunityToolkit.Maui;
using ImageDuplicateSearcher.Application.Common.Settings;
using ImageDuplicateSearcher.Application.Image.Interfaces;
using ImageDuplicateSearcher.Application.Image.Services;
using ImageDuplicateSearcher.Application.Reporting.Interfaces;
using ImageDuplicateSearcher.Application.Reporting.Services;
using ImageDuplicationSearcher.Desktop;
using ImageDuplicationSearcher.Desktop.Services;
using Microsoft.Extensions.Configuration;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        builder.Configuration.AddJsonStream(typeof(MauiProgram).Assembly.GetManifestResourceStream("appsettings.json")!);
        builder.Services.Configure<ImageDuplicationOptions>(builder.Configuration.GetSection(ImageDuplicationOptions.Section));
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
