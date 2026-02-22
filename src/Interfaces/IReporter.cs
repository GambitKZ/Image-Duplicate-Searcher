namespace ImageDuplicateSearcher.Interfaces;

/// <summary>
/// Defines the contract for reporting duplicate image groups.
/// </summary>
public interface IReporter
{
    /// <summary>
    /// Reports the duplicate image groups to the console and saves to a JSON file.
    /// </summary>
    /// <param name="groups">A dictionary where the key is the hash and the value is a list of file paths with that hash.</param>
    void ReportDuplicates(Dictionary<ulong, List<string>> groups);
}
