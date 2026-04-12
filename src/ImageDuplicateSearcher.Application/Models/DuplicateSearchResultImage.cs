using System.Text.Json.Serialization;

namespace ImageDuplicateSearcher.Application.Models;

/// <summary>
/// Represents an image entry in a duplicate search result group.
/// Maps directly to the JSON structure produced by the Reporter service.
/// </summary>
public record DuplicateSearchResultImage
{
    /// <summary>
    /// Gets or sets the file path of the image.
    /// </summary>
    [JsonPropertyName("path")]
    public required string Path { get; set; }

    /// <summary>
    /// Gets or sets the file size in megabytes.
    /// </summary>
    [JsonPropertyName("sizeMB")]
    public required double SizeMB { get; set; }

    /// <summary>
    /// Gets a value indicating whether this image has been marked as deleted in the current session.
    /// This property is not part of the source JSON; it exists only in memory for session state.
    /// </summary>
    [JsonIgnore]
    public bool IsDeleted { get; set; }
}
