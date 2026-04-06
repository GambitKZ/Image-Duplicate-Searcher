using System.Text.Json;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using ImageDuplicateSearcher.Application.Settings;
using Microsoft.Extensions.Options;

namespace ImageDuplicateSearcher.Application.Services;

/// <summary>
/// UI-aware reporter that raises log messages for the UI and still persists duplicate groups to JSON.
/// </summary>
public class UiReporter : IReporter
{
    private ImageDuplicationOptions _options;

    public event Action<string>? OnLogMessage;

    public UiReporter(IOptions<ImageDuplicationOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Update runtime options (used by Desktop UI to override output path or formats).
    /// </summary>
    public void UpdateOptions(ImageDuplicationOptions options)
    {
        _options = options;
    }

    public void ReportDuplicates(Dictionary<ulong, List<ImageModel>> groups)
    {
        var duplicates = groups.Where(g => g.Value.Count > 1).ToDictionary(g => g.Key, g => g.Value);

        if (duplicates.Count == 0)
        {
            OnLogMessage?.Invoke("No duplicate images found.");
            return;
        }

        OnLogMessage?.Invoke("Duplicate Image Groups:");

        int groupNumber = 1;
        foreach (var group in duplicates)
        {
            OnLogMessage?.Invoke($"Group {groupNumber} (Hash: {group.Key})");
            foreach (var image in group.Value)
            {
                double sizeMB = image.FileSize / (1024.0 * 1024.0);
                OnLogMessage?.Invoke($"  - {image.FilePath} ({sizeMB:F2} MB)");
            }
            groupNumber++;
        }

        // Serialize to JSON and save to file (reuse existing format)
        var jsonData = duplicates.Select(g => new { hash = g.Key, images = g.Value.Select(img => new { path = img.FilePath, sizeMB = img.FileSize / (1024.0 * 1024.0) }).ToArray() }).ToArray();
        var json = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
        try
        {
            File.WriteAllText(_options.OutputFilePath, json);
            OnLogMessage?.Invoke($"Duplicate groups saved to {_options.OutputFilePath}");
        }
        catch (Exception ex)
        {
            OnLogMessage?.Invoke($"Failed to save duplicate groups: {ex.Message}");
        }
    }
}
