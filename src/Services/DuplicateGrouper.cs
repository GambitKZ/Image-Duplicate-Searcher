using ImageDuplicateSearcher.Interfaces;

namespace ImageDuplicateSearcher.Services;

/// <summary>
/// TODO: Remove it.
/// Implements the duplicate grouper to group images by their computed hash.
/// </summary>
public class DuplicateGrouper : IDuplicateGrouper
{
    /// <summary>
    /// Groups images by their computed hash.
    /// </summary>
    /// <param name="images">An enumerable collection of tuples containing the file path and computed hash for each image.</param>
    /// <returns>A dictionary where the key is the hash and the value is a list of file paths with that hash.</returns>
    public Dictionary<ulong, List<string>> GroupByHash(IEnumerable<(string FilePath, ulong Hash)> images)
    {
        var groups = new Dictionary<ulong, List<string>>();

        foreach (var (filePath, hash) in images)
        {
            if (!groups.ContainsKey(hash))
            {
                groups[hash] = new List<string>();
            }
            groups[hash].Add(filePath);
        }

        return groups;
    }
}
