namespace ImageDuplicateSearcher.Application.Models;

/// <summary>
/// Result returned by <see cref="IImageDisplayManager"/> describing the bytes to display,
/// whether the payload is a placeholder, and an optional human-readable reason for the placeholder.
/// </summary>
public sealed record ImageDisplayResult
{
    /// <summary>
    /// Gets the bytes to render in the UI. For placeholders this will contain
    /// the placeholder image bytes.
    /// </summary>
    public required byte[] ImageBytes { get; init; }

    /// <summary>
    /// True when the returned bytes represent a placeholder rather than the original image.
    /// </summary>
    public bool IsPlaceholder { get; init; }

    /// <summary>
    /// Optional short reason describing why a placeholder was returned (for example "Missing file" or "Unreadable image").
    /// </summary>
    public string? Reason { get; init; }
}
