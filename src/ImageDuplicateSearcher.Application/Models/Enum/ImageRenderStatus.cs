namespace ImageDuplicateSearcher.Application.Models;

/// <summary>
/// Describes the render status of an image tile.
/// </summary>
public enum ImageRenderStatus
{
    /// <summary>
    /// Image bytes are available and can be rendered.
    /// </summary>
    Available,

    /// <summary>
    /// Image file was missing on disk.
    /// </summary>
    Missing,

    /// <summary>
    /// Image file exists but could not be decoded/read.
    /// </summary>
    Unreadable
}
