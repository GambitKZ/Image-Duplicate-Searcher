namespace ImageDuplicateSearcher.Application.Common.Settings;

/// <summary>
/// Options for image duplication configuration.
/// </summary>
public record ImageDuplicationOptions
{
    /// <summary>
    /// The section name for configuration binding.
    /// </summary>
    public const string Section = "ImageDuplicationOptions";

    /// <summary>
    /// Gets the directory to scan for images.
    /// </summary>
    public string ImageDirectory { get; init; } = string.Empty;

    /// <summary>
    /// Gets the path to the output file for duplicate results.
    /// </summary>
    public string OutputFilePath { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of supported image formats.
    /// </summary>
    public string[] SupportedFormats { get; init; } = [];
}
