using System.Runtime.InteropServices;
using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using Microsoft.VisualBasic.FileIO;

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

            // For Windows send to REcycle Bin, for other OSes perform direct deletion.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                await Task.Run(() =>
                    FileSystem.DeleteFile(path,
                        UIOption.OnlyErrorDialogs,
                        RecycleOption.SendToRecycleBin)
                ).ConfigureAwait(false);
            }
            else
            {
                await Task.Run(() => File.Delete(path)).ConfigureAwait(false);
            }

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
