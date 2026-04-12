using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Interfaces;

/// <summary>
/// Defines the contract for loading and deserializing JSON search results from the Desktop application.
/// </summary>
public interface IResultsLoader
{
    /// <summary>
    /// Loads and deserializes a JSON results file produced by the Console Reporter service.
    /// </summary>
    /// <param name="jsonFilePath">The absolute file path to the JSON results file.</param>
    /// <returns>An array of duplicate search result groups, where each group contains images with matching hashes.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the JSON structure is invalid or missing required fields.</exception>
    /// <exception cref="System.Text.Json.JsonException">Thrown when JSON deserialization fails.</exception>
    Task<DuplicateSearchResult[]> LoadResultsAsync(string jsonFilePath);

    /// <summary>
    /// Retrieves a specific duplicate result group by hash value (optional operation for navigation support).
    /// </summary>
    /// <param name="hash">The hash value to search for.</param>
    /// <returns>The duplicate result matching the hash, or null if not found.</returns>
    Task<DuplicateSearchResult?> GetResultByHashAsync(ulong hash);
}
