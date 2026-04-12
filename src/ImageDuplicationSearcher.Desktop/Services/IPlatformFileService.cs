using System.Threading.Tasks;

namespace ImageDuplicationSearcher.Desktop.Services;

/// <summary>
/// Platform abstraction for file-picking and minimal permission checks.
/// Implementations live in the Desktop project and are registered in DI.
/// </summary>
public interface IPlatformFileService
{
    /// <summary>
    /// Opens a platform file picker and returns a <see cref="PlatformFileResult"/>,
    /// or null when the user cancels or selection fails.
    /// </summary>
    Task<PlatformFileResult?> PickJsonFileAsync();

    /// <summary>
    /// Ensures any minimal runtime read permissions required by the platform are available.
    /// Returns true when the app can proceed to access the selected file.
    /// </summary>
    Task<bool> EnsureReadPermissionAsync();

    /// <summary>
    /// Attempts to delete the original file identified by a platform URI (e.g., content://)
    /// or filesystem path. Returns true when deletion succeeds.
    /// </summary>
    Task<bool> TryDeleteOriginalAsync(string? originalUri);
}
