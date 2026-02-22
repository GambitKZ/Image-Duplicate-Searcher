namespace ImageDuplicateSearcher.Models;

internal record ImageModel
{
    public string FilePath { get; set; }
    public long FileSize { get; set; }
}
