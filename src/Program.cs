using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// Create the host with default configuration setup
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Add JSON configuration file (required)
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        // Add user secrets for sensitive configuration
        config.AddUserSecrets<Program>();
    })
    .ConfigureServices((hostContext, services) =>
    {
        // Configure ImageDuplicationOptions from configuration
        services.Configure<ImageDuplicationOptions>(hostContext.Configuration.GetSection(ImageDuplicationOptions.Section));
    })
    .Build();

// Placeholder: Configuration is now available via host.Services.GetRequiredService<IConfiguration>()
Console.WriteLine("Configuration loaded successfully.");

// Demonstrate direct options access
var options = host.Services.GetRequiredService<IOptions<ImageDuplicationOptions>>().Value;
Console.WriteLine($"Direct options: Image directory = {options.ImageDirectory}, Thumbnail size = {options.ThumbnailSize}");

await host.RunAsync();
