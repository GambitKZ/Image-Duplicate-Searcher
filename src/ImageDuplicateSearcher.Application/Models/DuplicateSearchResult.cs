using System.Text.Json.Serialization;

namespace ImageDuplicateSearcher.Application.Models;

/// <summary>
/// Represents a group of duplicate images from a search result.
/// Maps directly to the JSON structure produced by the Reporter service.
/// </summary>
public record DuplicateSearchResult
{
    /// <summary>
    /// Gets or sets the perceptual hash value as a string representation.
    /// </summary>
    [JsonPropertyName("hash")]
    public required ulong Hash { get; set; }

    /// <summary>
    /// Gets or sets the collection of images that share this hash.
    /// </summary>
    [JsonPropertyName("images")]
    public required IReadOnlyList<DuplicateSearchResultImage> Images { get; set; }
}
