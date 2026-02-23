namespace ImageDuplicateSearcher.Application.Interfaces;

public interface IImageProcessor
{

    /// <summary>
    /// Get a list of the images that will be processed.
    /// </summary>
    /// <returns>List of path to the images that needs to be processed.</returns>
    IEnumerable<string> GetImageList();

    /// <summary>
    /// Generates a thumbnail for the specified image file.
    /// </summary>
    /// <param name="filePath">The path to the image file.</param>
    /// <returns>A MemoryStream containing the JPEG thumbnail.</returns>
    MemoryStream GenerateThumbnail(string filePath);

    /// <summary>
    /// Computes a perceptual hash for the given thumbnail image.
    /// </summary>
    /// <param name="thumbnail">The thumbnail image as a MemoryStream.</param>
    /// <returns>The perceptual hash as a 64-bit unsigned integer.</returns>
    ulong ComputePerceptualHash(MemoryStream thumbnail);
}
