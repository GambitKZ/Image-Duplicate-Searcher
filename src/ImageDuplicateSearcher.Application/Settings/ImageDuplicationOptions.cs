namespace ImageDuplicateSearcher.Application.Settings;

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
    /// Gets or sets the path to the output file for duplicate results.
    /// </summary>
    public string OutputFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of supported image formats.
    /// </summary>
    public List<string> SupportedFormats { get; set; } = new List<string>();

    /// <summary>
    /// Parse a delimited string of formats into the normalized SupportedFormats list.
    /// Accepts separators like ';' and ',' and ensures each extension has a leading dot,
    /// is trimmed, lower-cased and unique.
    /// </summary>
    /// <param name="formats">Delimited formats string, e.g. "jpg;png;.bmp"</param>
    public void SetSupportedFormatsFromString(string? formats)
    {
        SupportedFormats.Clear();
        if (string.IsNullOrWhiteSpace(formats))
            return;

        var separators = new[] { ';', ',' };
        var tokens = formats.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var tok in tokens)
        {
            var t = tok.Trim();
            if (string.IsNullOrEmpty(t))
                continue;
            if (!t.StartsWith('.'))
                t = "." + t;
            t = t.ToLowerInvariant();
            if (seen.Add(t))
                SupportedFormats.Add(t);
        }
    }
}
