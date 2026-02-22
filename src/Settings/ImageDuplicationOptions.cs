namespace ImageDuplicateSearcher.Settings;

/// <summary>
/// Options for image duplication configuration.
/// </summary>
public class ImageDuplicationOptions
{
    /// <summary>
    /// The section name for configuration binding.
    /// </summary>
    public const string Section = "ImageDuplicationOptions";

    /// <summary>
    /// Gets or sets the directory to scan for images.
    /// </summary>
    public string ImageDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the thumbnail in pixels (square).
    /// Looks like it is useless.
    /// </summary>
    public int ThumbnailSize { get; set; }

    /// <summary>
    /// Gets or sets the path to the output file for duplicate results.
    /// </summary>
    public string OutputFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of supported image formats.
    /// </summary>
    public List<string> SupportedFormats { get; set; } = new List<string>();
}
