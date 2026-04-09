using System.Threading.Tasks;
using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Interfaces;

/// <summary>
/// Handles loading and validation of image files for UI rendering.
/// Implementations return either the actual image bytes or a deterministic placeholder with a reason.
/// This interface intentionally avoids UI framework types so it can live in the Application layer.
/// </summary>
public interface IImageDisplayManager
{
    /// <summary>
    /// Loads image display data (image bytes or placeholder) for the specified file path.
    /// Implementations SHOULD validate image format and avoid throwing for common I/O issues;
    /// instead, return an <see cref="ImageDisplayResult"/> with <see cref="ImageDisplayResult.IsPlaceholder"/> set.
    /// </summary>
    /// <param name="imagePath">Absolute or relative file path to the image.</param>
    /// <returns>An <see cref="ImageDisplayResult"/> describing bytes to render and any placeholder reason.</returns>
    Task<ImageDisplayResult> GetImageDisplayAsync(string imagePath);

    /// <summary>
    /// Loads raw image bytes for the specified file path.
    /// Implementations MAY throw <see cref="System.IO.FileNotFoundException"/> or
    /// <see cref="System.IO.IOException"/> if the file is not accessible.
    /// </summary>
    /// <param name="imagePath">Absolute or relative file path to the image.</param>
    /// <returns>Raw image bytes.</returns>
    Task<byte[]> GetImageBytesAsync(string imagePath);

    /// <summary>
    /// Determines whether the image file exists and is readable.
    /// </summary>
    /// <param name="imagePath">Absolute or relative file path to the image.</param>
    /// <returns>True if the file is accessible and likely readable; otherwise false.</returns>
    bool IsImageAccessible(string imagePath);
}
