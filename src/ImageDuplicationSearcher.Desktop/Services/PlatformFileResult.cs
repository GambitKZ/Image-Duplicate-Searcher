namespace ImageDuplicationSearcher.Desktop.Services;

/// <summary>
/// Result returned by platform file picker adapters.
/// Path: local filesystem path (may be a temp/cache copy).
/// IsTemporaryCopy: true when the file was copied into app cache.
/// OriginalUri: optional original content:// URI (Android) when available.
/// </summary>
public sealed class PlatformFileResult
{
    public string Path { get; set; } = string.Empty;
    public bool IsTemporaryCopy { get; set; }
    public string? OriginalUri { get; set; }
}
