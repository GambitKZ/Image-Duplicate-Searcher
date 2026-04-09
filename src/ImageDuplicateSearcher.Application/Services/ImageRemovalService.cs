using System;
using System.IO;
using System.Threading.Tasks;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;

namespace ImageDuplicateSearcher.Application.Services;

/// <summary>
/// Attempts safe deletion of image files and maps common failures to <see cref="RemovalResult"/>.
/// This service performs file-system operations only and MUST NOT modify source JSON files.
/// </summary>
public class ImageRemovalService : IImageRemovalService
{
    /// <inheritdoc />
    public async Task<RemovalResult> RemoveImageAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return RemovalResult.NotFound;
        }

        try
        {
            if (!File.Exists(path))
            {
                return RemovalResult.NotFound;
            }

            // Perform the delete on a background thread to avoid blocking callers.
            await Task.Run(() => File.Delete(path)).ConfigureAwait(false);

            return RemovalResult.Success;
        }
        catch (UnauthorizedAccessException)
        {
            return RemovalResult.Unauthorized;
        }
        catch (DirectoryNotFoundException)
        {
            return RemovalResult.NotFound;
        }
        catch (FileNotFoundException)
        {
            return RemovalResult.NotFound;
        }
        catch (IOException)
        {
            // IO exceptions commonly indicate the file is locked/in-use.
            return RemovalResult.Locked;
        }
        catch (Exception)
        {
            return RemovalResult.UnknownError;
        }
    }
}
