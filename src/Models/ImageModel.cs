namespace ImageDuplicateSearcher.Models;

public record ImageModel
{
    public string FilePath { get; set; }
    public long FileSize { get; set; }
}
