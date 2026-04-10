using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace ImageDuplicationSearcher.Desktop.Services;

/// <summary>
/// Windows implementation of <see cref="IPlatformFileService"/>.
/// Uses MAUI FilePicker for user-driven selection; Windows does not require runtime
/// storage permissions for the picker flow.
/// </summary>
public class WindowsPlatformFileService : IPlatformFileService
{
    public Task<bool> EnsureReadPermissionAsync()
    {
        // FilePicker on Windows does not require runtime permissions.
        return Task.FromResult(true);
    }

    public async Task<PlatformFileResult?> PickJsonFileAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select JSON Results File"
            }).ConfigureAwait(false);

            if (result is null) return null;

            return new PlatformFileResult
            {
                Path = result.FullPath ?? string.Empty,
                IsTemporaryCopy = false,
                OriginalUri = null
            };
        }
        catch (Exception)
        {
            // Swallow and return null for caller to show friendly message.
            return null;
        }
    }

    public Task<bool> TryDeleteOriginalAsync(string? originalUri)
    {
        if (string.IsNullOrWhiteSpace(originalUri)) return Task.FromResult(false);

        try
        {
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
