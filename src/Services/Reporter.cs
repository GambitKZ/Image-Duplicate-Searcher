using System.Text.Json;
using ImageDuplicateSearcher.Interfaces;
using ImageDuplicateSearcher.Settings;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace ImageDuplicateSearcher.Services;

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
    /// <param name="groups">A dictionary where the key is the hash and the value is a list of file paths with that hash.</param>
    public void ReportDuplicates(Dictionary<ulong, List<string>> groups)
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
                AnsiConsole.MarkupLine($"  - {Markup.Escape(image)}");
            }
            groupNumber++;
        }

        // Serialize to JSON and save to file
        var jsonData = duplicates.Select(g => new { hash = g.Key, images = g.Value }).ToArray();
        var json = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_options.OutputFilePath, json);
        AnsiConsole.MarkupLine($"[green]Duplicate groups saved to {_options.OutputFilePath}[/]");
    }
}
