using System.Text.Json;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Services;

/// <summary>
/// Service for loading and deserializing JSON search results from the Desktop application.
/// </summary>
public class ResultsLoader : IResultsLoader
{
    private DuplicateSearchResult[]? _cachedResults;

    /// <summary>
    /// Loads and deserializes a JSON results file produced by the Console Reporter service.
    /// </summary>
    /// <param name="jsonFilePath">The absolute file path to the JSON results file.</param>
    /// <returns>An array of duplicate search result groups.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the JSON structure is invalid or missing required fields.</exception>
    /// <exception cref="System.Text.Json.JsonException">Thrown when JSON deserialization fails.</exception>
    public async Task<DuplicateSearchResult[]> LoadResultsAsync(string jsonFilePath)
    {
        if (string.IsNullOrWhiteSpace(jsonFilePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(jsonFilePath));
        }

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"Results file not found: {jsonFilePath}");
        }

        try
        {
            string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var results = JsonSerializer.Deserialize<DuplicateSearchResult[]>(jsonContent, options);

            if (results is null)
            {
                throw new InvalidOperationException("JSON deserialization produced a null result array.");
            }

            ValidateResults(results);
            _cachedResults = results;
            return results;
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Invalid JSON format in results file: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Retrieves a specific duplicate result group by hash value.
    /// </summary>
    /// <param name="hash">The hash value to search for.</param>
    /// <returns>The duplicate result matching the hash, or null if not found.</returns>
    public Task<DuplicateSearchResult?> GetResultByHashAsync(ulong hash)
    {
        var result = _cachedResults?.FirstOrDefault(r => r.Hash == hash);
        return Task.FromResult(result);
    }

    /// <summary>
    /// Validates the structure and content of loaded results.
    /// </summary>
    /// <param name="results">The results array to validate.</param>
    /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
    private static void ValidateResults(DuplicateSearchResult[] results)
    {
        if (results.Length == 0)
        {
            throw new InvalidOperationException("Results array is empty.");
        }

        for (int i = 0; i < results.Length; i++)
        {
            var result = results[i];

            if (result is null)
            {
                throw new InvalidOperationException($"Result at index {i} is null.");
            }

            // Hash is a numeric ulong produced by the Reporter; zero is a valid hash value, so we only check for presence.

            if (result.Images is null || result.Images.Count == 0)
            {
                throw new InvalidOperationException($"Result at index {i} (hash: {result.Hash}) has no images.");
            }

            for (int j = 0; j < result.Images.Count; j++)
            {
                var image = result.Images[j];

                if (image is null)
                {
                    throw new InvalidOperationException($"Image at index {j} in result {i} (hash: {result.Hash}) is null.");
                }

                if (string.IsNullOrWhiteSpace(image.Path))
                {
                    throw new InvalidOperationException($"Image at index {j} in result {i} (hash: {result.Hash}) has a null or empty path.");
                }

                if (image.SizeMB < 0)
                {
                    throw new InvalidOperationException($"Image at index {j} in result {i} (hash: {result.Hash}) has a negative file size.");
                }
            }
        }
    }
}
