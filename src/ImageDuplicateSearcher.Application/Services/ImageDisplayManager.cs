using System.Collections.Concurrent;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using ImageDuplicateSearcher.Application.Resources;
using SixLabors.ImageSharp;

namespace ImageDuplicateSearcher.Application.Services;

/// <summary>
/// Application-layer implementation that loads image bytes for the UI and
/// returns a deterministic placeholder when images are missing or unreadable.
/// Uses SixLabors.ImageSharp only for validation and placeholder generation.
/// </summary>
public class ImageDisplayManager : IImageDisplayManager
{
    private readonly ConcurrentDictionary<string, ImageDisplayResult> _cache = new();
    private readonly byte[] _placeholderBytes;

    public ImageDisplayManager()
    {
        _placeholderBytes = PlaceholderResources.PlaceholderPngBytes;
    }

    public async Task<ImageDisplayResult> GetImageDisplayAsync(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return new ImageDisplayResult
            {
                ImageBytes = _placeholderBytes,
                IsPlaceholder = true,
                Reason = "Missing file"
            };
        }

        if (_cache.TryGetValue(imagePath, out var cached))
        {
            return cached;
        }

        try
        {
            if (!File.Exists(imagePath))
            {
                var miss = new ImageDisplayResult { ImageBytes = _placeholderBytes, IsPlaceholder = true, Reason = "Missing file" };
                _cache.TryAdd(imagePath, miss);
                return miss;
            }

            byte[] bytes = await File.ReadAllBytesAsync(imagePath).ConfigureAwait(false);

            try
            {
                // Validate decodability using ImageSharp. We don't modify the bytes; validation is sufficient.
                using var _ = Image.Load(bytes);
            }
            catch (Exception)
            {
                var unreadable = new ImageDisplayResult { ImageBytes = _placeholderBytes, IsPlaceholder = true, Reason = "Unreadable image" };
                _cache.TryAdd(imagePath, unreadable);
                return unreadable;
            }

            var ok = new ImageDisplayResult { ImageBytes = bytes, IsPlaceholder = false, Reason = null };
            _cache.TryAdd(imagePath, ok);
            return ok;
        }
        catch (UnauthorizedAccessException)
        {
            var denied = new ImageDisplayResult { ImageBytes = _placeholderBytes, IsPlaceholder = true, Reason = "Permission denied" };
            _cache.TryAdd(imagePath, denied);
            return denied;
        }
        catch (Exception)
        {
            var error = new ImageDisplayResult { ImageBytes = _placeholderBytes, IsPlaceholder = true, Reason = "Unreadable image" };
            _cache.TryAdd(imagePath, error);
            return error;
        }
    }

    public async Task<byte[]> GetImageBytesAsync(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            throw new FileNotFoundException("Image path is null or empty.");
        }

        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException(imagePath);
        }

        return await File.ReadAllBytesAsync(imagePath).ConfigureAwait(false);
    }

    public bool IsImageAccessible(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath)) return false;

        try
        {
            if (!File.Exists(imagePath)) return false;

            using var stream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
