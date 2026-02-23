namespace ImageDuplicateSearcher.Application.Models;

public record ImageModel
{
    public required string FilePath { get; set; }
    public long FileSize { get; set; }
}
