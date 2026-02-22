using ImageDuplicateSearcher.Interfaces;
using ImageDuplicateSearcher.Settings;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace ImageDuplicateSearcher.Services;

public class WorkflowService
{
    private readonly ImageDuplicationOptions _options;
    private readonly IImageProcessor _imageProcessor;
    private readonly IReporter _reporter;

    public WorkflowService(IOptions<ImageDuplicationOptions> options, IImageProcessor imageProcessor, IReporter reporter)
    {
        _options = options.Value;
        _imageProcessor = imageProcessor;
        _reporter = reporter;
    }

    public void ExecuteWorkflow()
    {
        // Can thtow an exception
        var images = _imageProcessor.GetImageList();

        var imageNumber = images.Count();

        AnsiConsole.MarkupLine($"Path [green]{Markup.Escape(_options.ImageDirectory)}[/] contains [yellow]{imageNumber}[/] elements");

        Dictionary<ulong, List<string>> duplicateDictionary = new Dictionary<ulong, List<string>>();

        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask("Processing images...", maxValue: imageNumber);

                foreach (var imagePath in images)
                {
                    using var thumbnailStream = _imageProcessor.GenerateThumbnail(imagePath);

                    var hash = _imageProcessor.ComputePerceptualHash(thumbnailStream);

                    //var imageModel = new ImageModel
                    //{
                    //    FilePath = imagePath,
                    //    FileSize =
                    //};

                    if (duplicateDictionary.TryGetValue(hash, out var value))
                    {
                        value.Add(imagePath);
                    }
                    else
                    {
                        duplicateDictionary[hash] = new List<string> { imagePath };
                    }
                }
            });

        _reporter.ReportDuplicates(duplicateDictionary);
    }
}
