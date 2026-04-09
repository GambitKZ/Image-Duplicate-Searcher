using System;

namespace ImageDuplicateSearcher.Application.Resources;

/// <summary>
/// Provides embedded placeholder image bytes used when real images cannot be displayed.
/// The placeholder is a small 1x1 PNG encoded as Base64 to keep the repository text-friendly.
/// </summary>
public static class PlaceholderResources
{
    private const string PlaceholderBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR4nGNgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII=";

    /// <summary>
    /// Returns PNG bytes for the embedded placeholder image.
    /// </summary>
    public static byte[] PlaceholderPngBytes => Convert.FromBase64String(PlaceholderBase64);
}
