using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Interfaces;

/// <summary>
/// Defines a contract for attempting to remove an image file from disk.
/// Implementations should perform file-system deletion only and return a <see cref="RemovalResult"/> describing the outcome.
/// The implementation MUST NOT modify the source JSON file.
/// </summary>
public interface IImageRemovalService
{
    /// <summary>
    /// Attempts to remove the file at the specified path.
    /// </summary>
    /// <param name="path">Absolute path to the file to remove.</param>
    /// <returns>A <see cref="RemovalResult"/> indicating the result of the operation.</returns>
    Task<RemovalResult> RemoveImageAsync(string path);
}
