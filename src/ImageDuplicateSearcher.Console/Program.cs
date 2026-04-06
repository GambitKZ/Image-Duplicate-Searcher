using ImageDuplicateSearcher.Application.Extensions;
using ImageDuplicateSearcher.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>();

builder.Services.ConfigureServices(builder.Configuration);

using IHost host = builder.Build();

var workflowService = host.Services.GetRequiredService<WorkflowService>();

workflowService.ExecuteWorkflow();
