using System.Text.Json;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using ImageDuplicateSearcher.Application.Settings;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace ImageDuplicateSearcher.Application.Services;

/// <summary>
/// Service for reporting duplicate image groups to the console and saving to a JSON file.
/// </summary>
public class Reporter : IReporter
{
    private readonly ImageDuplicationOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="Reporter"/> class.
    /// </summary>
    /// <param name="options">The image duplication options.</param>
    public Reporter(IOptions<ImageDuplicationOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Reports the duplicate image groups to the console and saves to a JSON file.
    /// </summary>
    /// <param name="groups">A dictionary where the key is the hash and the value is a list of image models with that hash.</param>
    public void ReportDuplicates(Dictionary<ulong, List<ImageModel>> groups)
    {
        // Filter for groups with more than one image (duplicates)
        var duplicates = groups.Where(g => g.Value.Count > 1).ToDictionary(g => g.Key, g => g.Value);

        if (duplicates.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No duplicate images found.[/]");
            return;
        }

        AnsiConsole.MarkupLine("[bold green]Duplicate Image Groups:[/]");

        int groupNumber = 1;
        foreach (var group in duplicates)
        {
            AnsiConsole.MarkupLine($"[bold]Group {groupNumber} (Hash: {group.Key})[/]");
            foreach (var image in group.Value)
            {
                double sizeMB = image.FileSize / (1024.0 * 1024.0);
                AnsiConsole.MarkupLine($"  - {Markup.Escape(image.FilePath)} ({sizeMB:F2} MB)");
            }
            groupNumber++;
        }

        // Serialize to JSON and save to file
        var jsonData = duplicates.Select(g => new { hash = g.Key, images = g.Value.Select(img => new { path = img.FilePath, sizeMB = img.FileSize / (1024.0 * 1024.0) }).ToArray() }).ToArray();
        var json = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_options.OutputFilePath, json);
        AnsiConsole.MarkupLine($"[green]Duplicate groups saved to {_options.OutputFilePath}[/]");
    }
}
