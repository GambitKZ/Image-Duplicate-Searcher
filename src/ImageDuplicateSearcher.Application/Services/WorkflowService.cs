using ImageDuplicateSearcher.Application.Interfaces;
using ImageDuplicateSearcher.Application.Models;
using ImageDuplicateSearcher.Application.Settings;
using Microsoft.Extensions.Options;
using Spectre.Console;
using System.Threading;
using System.Threading.Tasks;

namespace ImageDuplicateSearcher.Application.Services;

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
        // Keep backward-compatible synchronous API by running the async method and blocking.
        ExecuteWorkflowAsync(null, null, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task ExecuteWorkflowAsync(ImageDuplicationOptions? runtimeOptions = null, IProgress<string>? progress = null, CancellationToken cancellationToken = default)
    {
        // Determine which options to use for listing/processing
        var optionsToUse = runtimeOptions ?? _options;

        IEnumerable<string> images;
        if (runtimeOptions != null)
        {
            if (!Directory.Exists(optionsToUse.ImageDirectory))
            {
                throw new FileNotFoundException($"Folder {optionsToUse.ImageDirectory} is empty");
            }

            images = Directory.GetFiles(optionsToUse.ImageDirectory).Where(i => optionsToUse.SupportedFormats.Contains(Path.GetExtension(i).ToLowerInvariant()));
        }
        else
        {
            images = _imageProcessor.GetImageList();
        }

        var imageNumber = images.Count();

        if (progress == null)
        {
            AnsiConsole.MarkupLine($"Path [green]{Markup.Escape(optionsToUse.ImageDirectory)}[/] contains [yellow]{imageNumber}[/] elements");
        }
        else
        {
            progress.Report($"Path {optionsToUse.ImageDirectory} contains {imageNumber} elements");
        }

        var duplicateDictionary = new Dictionary<ulong, List<ImageModel>>();

        if (progress == null)
        {
            // Use Spectre.Console progress for console scenarios
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask("Processing images...", maxValue: imageNumber);

                    foreach (var imagePath in images)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        using var thumbnailStream = _imageProcessor.GenerateThumbnail(imagePath);

                        var hash = _imageProcessor.ComputePerceptualHash(thumbnailStream);

                        var imageModel = new ImageModel
                        {
                            FilePath = imagePath,
                            FileSize = new FileInfo(imagePath).Length
                        };

                        if (duplicateDictionary.TryGetValue(hash, out var value))
                        {
                            value.Add(imageModel);
                        }
                        else
                        {
                            duplicateDictionary[hash] = new List<ImageModel> { imageModel };
                        }

                        task.Increment(1);
                    }
                });
        }
        else
        {
            // UI scenario: push simple text progress messages
            int processed = 0;
            foreach (var imagePath in images)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using var thumbnailStream = _imageProcessor.GenerateThumbnail(imagePath);

                var hash = _imageProcessor.ComputePerceptualHash(thumbnailStream);

                var imageModel = new ImageModel
                {
                    FilePath = imagePath,
                    FileSize = new FileInfo(imagePath).Length
                };

                if (duplicateDictionary.TryGetValue(hash, out var value))
                {
                    value.Add(imageModel);
                }
                else
                {
                    duplicateDictionary[hash] = new List<ImageModel> { imageModel };
                }

                processed++;
                progress.Report($"Processed {processed}/{imageNumber}: {Path.GetFileName(imagePath)}");
            }
        }

        // If runtimeOptions provided and reporter supports runtime update, pass them through
        if (runtimeOptions != null && _reporter is ImageDuplicateSearcher.Application.Services.UiReporter ui)
        {
            ui.UpdateOptions(runtimeOptions);
        }

        // Report duplicates (this will use the reporter implementation registered in DI)
        _reporter.ReportDuplicates(duplicateDictionary);
    }
}
