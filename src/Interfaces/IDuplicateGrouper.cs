namespace ImageDuplicateSearcher.Interfaces;

/// <summary>
/// Defines the contract for grouping duplicate images by their computed hash.
/// </summary>
public interface IDuplicateGrouper
{
    /// <summary>
    /// Groups images by their computed hash.
    /// </summary>
    /// <param name="images">An enumerable collection of tuples containing the file path and computed hash for each image.</param>
    /// <returns>A dictionary where the key is the hash and the value is a list of file paths with that hash.</returns>
    Dictionary<ulong, List<string>> GroupByHash(IEnumerable<(string FilePath, ulong Hash)> images);
}
