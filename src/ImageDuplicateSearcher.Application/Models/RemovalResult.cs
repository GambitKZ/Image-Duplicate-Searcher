namespace ImageDuplicateSearcher.Application.Models;

/// <summary>
/// Canonical result codes for image removal attempts.
/// </summary>
public enum RemovalResult
{
    /// <summary>
    /// File removed successfully.
    /// </summary>
    Success,

    /// <summary>
    /// File was not found on disk.
    /// </summary>
    NotFound,

    /// <summary>
    /// Permission denied when attempting to delete the file.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// File is locked or in use by another process.
    /// </summary>
    Locked,

    /// <summary>
    /// An unknown or unexpected error occurred.
    /// </summary>
    UnknownError
}
