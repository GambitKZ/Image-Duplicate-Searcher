using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;

namespace ImageDuplicationSearcher.Desktop.Services;

#if ANDROID
/// <summary>
/// Android implementation of <see cref="IPlatformFileService"/>.
/// Uses MAUI FilePicker and runtime permission checks where applicable.
/// When SAF returns a content URI without a filesystem path, the adapter copies
/// the picked file into the app cache and returns the cache path.
/// </summary>
public class AndroidPlatformFileService : IPlatformFileService
{
    public async Task<bool> EnsureReadPermissionAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            return status == PermissionStatus.Granted;
        }
        catch
        {
            return false;
        }
    }

    public async Task<PlatformFileResult?> PickJsonFileAsync()
    {
        try
        {
            // Request a read permission if possible; FilePicker often grants access
            // without it but requesting improves behavior for direct file path access.
            await EnsureReadPermissionAsync().ConfigureAwait(false);

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select JSON Results File"
            }).ConfigureAwait(false);

            if (result is null) return null;

            // Prefer files with .json extension
            if (!result.FileName?.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ?? true)
            {
                return null;
            }

            // If platform provides a real filesystem path, return it.
            if (!string.IsNullOrWhiteSpace(result.FullPath))
            {
                return new PlatformFileResult { Path = result.FullPath, IsTemporaryCopy = false, OriginalUri = null };
            }

            // Otherwise copy into app cache and return the local path so callers can read it.
            using var stream = await result.OpenReadAsync().ConfigureAwait(false);
            var destName = $"picked-{Guid.NewGuid()}-{Path.GetFileName(result.FileName)}";
            var dest = Path.Combine(FileSystem.CacheDirectory, destName);

            using var destStream = File.Open(dest, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(destStream).ConfigureAwait(false);

            // We do not have a documented original content URI from the FilePicker API here,
            // so OriginalUri is left null. Callers should treat IsTemporaryCopy==true as a hint
            // that deleting the returned path will not remove the original file.
            return new PlatformFileResult { Path = dest, IsTemporaryCopy = true, OriginalUri = null };
        }
        catch
        {
            return null;
        }
    }

    public Task<bool> TryDeleteOriginalAsync(string? originalUri)
    {
        if (string.IsNullOrWhiteSpace(originalUri)) return Task.FromResult(false);

        try
        {
            // If it looks like a content URI, attempt to delete via ContentResolver.
            if (originalUri!.StartsWith("content://", System.StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var uri = Android.Net.Uri.Parse(originalUri);
                    var resolver = Android.App.Application.Context.ContentResolver;
                    var deleted = resolver.Delete(uri, null, null);
                    return Task.FromResult(deleted > 0);
                }
                catch
                {
                    return Task.FromResult(false);
                }
            }

            // Otherwise treat as filesystem path.
            if (File.Exists(originalUri))
            {
                File.Delete(originalUri);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
#endif
