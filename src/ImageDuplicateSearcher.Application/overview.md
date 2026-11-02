# ImageDuplicateSearcher.Application

Shared .NET 10 / C# 14 library for perceptual duplicate detection, JSON reporting,
result navigation, image display, and file removal. Consumed in-process by the
Console and Desktop applications; not an independently hosted service.

## Summary

- Scans one directory (not subdirectories), generates 32x32 JPEG thumbnails,
  computes 64-bit DCT-based perceptual hashes, and groups images by identical hash.
- Reports only groups containing more than one image; no similarity-distance
  threshold or byte-for-byte verification is applied.
- Supplies the Desktop's JSON loader, observable group navigator, cached image
  display data, and platform-aware file removal results.
- Uses ImageSharp for image processing, Spectre.Console for terminal output,
  Microsoft.Extensions options/DI abstractions, and System.Text.Json for files.

## Components

COMPONENT_NAME: WorkflowService
TYPE: Workflow
PURPOSE: Coordinates scanning, hashing, grouping, and reporting.
RESPONSIBILITIES:
  - Exposes synchronous ExecuteWorkflow and ExecuteWorkflowAsync entry points.
  - Accepts optional runtime options, IProgress<string>, and CancellationToken.
  - Uses IImageProcessor.GetImageList unless runtime options supply the directory.
  - Checks cancellation between images; sends all hash groups to IReporter.
SOURCE: [Services/WorkflowService.cs](Services/WorkflowService.cs)
CALLS: IImageProcessor, IReporter, IOptions<ImageDuplicationOptions>, Spectre.Console
CALLED_BY: Console startup; not currently wired into Desktop

COMPONENT_NAME: ImageProcessor
TYPE: Processing Service
PURPOSE: Implements IImageProcessor for image discovery and perceptual hashing.
RESPONSIBILITIES:
  - Filters top-level files by configured extensions.
  - Produces disposable thumbnail streams and computes a DCT/median-based hash.
SOURCE: [Services/ImageProcessor.cs](Services/ImageProcessor.cs)
CALLS: IOptions<ImageDuplicationOptions>, filesystem, ImageSharp
CALLED_BY: WorkflowService

COMPONENT_NAME: Reporter / UiReporter
TYPE: Output Adapters
PURPOSE: Implement IReporter using terminal output or UI log events plus JSON.
RESPONSIBILITIES:
  - Filter singleton hash groups and serialize duplicate groups to OutputFilePath.
  - Reporter writes Spectre.Console output and propagates save failures.
  - UiReporter exposes OnLogMessage, accepts UpdateOptions, and logs save failures.
SOURCE: [Services/Reporter.cs](Services/Reporter.cs), [Services/UiReporter.cs](Services/UiReporter.cs)
CALLS: IOptions<ImageDuplicationOptions>, System.Text.Json, filesystem
CALLED_BY: WorkflowService; UiReporter has no current host registration

COMPONENT_NAME: ResultsLoader
TYPE: Input Service
PURPOSE: Implements IResultsLoader to read and cache duplicate-result JSON.
RESPONSIBILITIES:
  - Deserialize case-insensitively and validate nonempty groups, image paths,
    and nonnegative sizes; zero is a valid hash.
  - Expose cached lookup by hash; wrap JsonException in InvalidOperationException.
SOURCE: [Services/ResultsLoader.cs](Services/ResultsLoader.cs)
CALLS: System.Text.Json, filesystem
CALLED_BY: Desktop MainPage

COMPONENT_NAME: DuplicateNavigator
TYPE: Stateful Navigation Service
PURPOSE: Implements IDuplicateNavigator and INotifyPropertyChanged.
RESPONSIBILITIES:
  - Track loaded groups, zero-based CurrentIndex (-1 when empty), and CurrentGroup.
  - Bound Previous/Next and translate one-based TryGoToGroup input.
  - Protect navigation state with a lock; raise notifications on the caller thread.
SOURCE: [Services/DuplicateNavigator.cs](Services/DuplicateNavigator.cs)
CALLED_BY: Desktop MainPage

COMPONENT_NAME: ImageDisplayManager
TYPE: Display Data Service
PURPOSE: Implements IImageDisplayManager without a MAUI dependency.
RESPONSIBILITIES:
  - Cache full image bytes and decoded dimensions by path for display.
  - Return ImageDisplayResult placeholders for missing, denied, or unreadable files.
  - Offer uncached raw-byte reads and an accessibility probe.
SOURCE: [Services/ImageDisplayManager.cs](Services/ImageDisplayManager.cs)
CALLS: filesystem, ImageSharp, embedded PlaceholderResources
CALLED_BY: Desktop ImageTileViewModel

COMPONENT_NAME: ImageRemovalService
TYPE: Filesystem Service
PURPOSE: Implements IImageRemovalService and maps deletion outcomes to RemovalResult.
RESPONSIBILITIES:
  - Use the Windows Recycle Bin; directly delete on other operating systems.
  - Return Success, NotFound, Unauthorized, Locked, or UnknownError.
  - Never modify the source results JSON.
SOURCE: [Services/ImageRemovalService.cs](Services/ImageRemovalService.cs)
CALLS: Microsoft.VisualBasic.FileIO.FileSystem, System.IO.File
CALLED_BY: Desktop ImageTileViewModel

## Dependency Registration and Wiring

REGISTRATION_FILE: [Extensions/ConfigureServices.cs](Extensions/ConfigureServices.cs)

`ServiceConfiguration.ConfigureServices(IServiceCollection, IConfiguration)` binds
the ImageDuplicationOptions section and registers:

| Lifetime | Service | Implementation |
| --- | --- | --- |
| Transient | WorkflowService | WorkflowService |
| Transient | IImageProcessor | ImageProcessor |
| Transient | IReporter | Reporter |

Console calls this extension. Desktop does not: it separately registers
IResultsLoader, IDuplicateNavigator, IImageDisplayManager, and IImageRemovalService
as singletons. UiReporter is available but unregistered in both hosts.

## Communication Channels

- All service calls are in-process. No HTTP, gRPC, message broker, or database.
- Console -> local JSON file -> Desktop is the cross-application handoff; paths
  inside that file must be accessible on the consuming device.
- Workflow progress uses Spectre.Console when progress is null, otherwise
  IProgress<string>. UiReporter additionally offers Action<string> log events.
- Desktop observes navigator PropertyChanged events and converts ImageDisplayResult
  bytes into MAUI ImageSource objects. RemovalResult is an enum, not an exception.

The JSON contract is an array of groups, with an unsigned 64-bit numeric hash,
image path, and size in MiB (bytes / 1024 / 1024, despite the sizeMB field name):

```json
[
  { "hash": 123, "images": [
    { "path": "C:/images/a.jpg", "sizeMB": 1.5 },
    { "path": "C:/images/b.jpg", "sizeMB": 1.5 }
  ] }
]
```

[Models/DuplicateSearchResult.cs](Models/DuplicateSearchResult.cs) and
[Models/DuplicateSearchResultImage.cs](Models/DuplicateSearchResultImage.cs) define
the read contract. IsDeleted is session-only and excluded from JSON. The loader
rejects an empty array but accepts a group with one image.

## Configuration Keys

Defined by [Settings/ImageDuplicationOptions.cs](Settings/ImageDuplicationOptions.cs),
consumed through IOptions<ImageDuplicationOptions>:

| Key | Model Default | Purpose |
| --- | --- | --- |
| ImageDuplicationOptions:ImageDirectory | Empty string | Top-level directory to scan |
| ImageDuplicationOptions:OutputFilePath | Empty string | JSON destination |
| ImageDuplicationOptions:SupportedFormats | Empty list | Allowed extensions, e.g. .jpg |

Hosts supply usable values. File extensions are lowercased before comparison, but
bound lists are not normalized automatically. SetSupportedFormatsFromString can
normalize comma/semicolon-separated input. No options validator is registered.
Runtime options update UiReporter only, not Reporter. IOptions values are not a
live-reload contract.

## Known Risks

- Identical perceptual hashes are candidates, not proof of duplicate content;
  visually different images can collide and similar images can hash differently.
- Workflow processing is serial and CPU-heavy. ExecuteWorkflowAsync currently has
  no awaited operations, so its name does not imply background execution.
- One unreadable image can abort the whole scan; cancellation is only checked
  between images. Output directories are not automatically created.
- Both reporters leave any existing output file untouched when no duplicates are
  found, potentially leaving stale results. Writes are non-atomic overwrites.
- ImageDisplayManager caches full-resolution bytes and failure results indefinitely,
  without size limits or invalidation after file replacement/deletion.
- Deletion is permanent outside Windows. ImageRemovalService also calls File.Delete
  again after its platform-specific deletion; if the path is recreated meanwhile,
  the replacement could be deleted. No allowed-root restriction is enforced.
- JSON validation checks structure, not path trust or actual file size/content;
  Desktop can act on any accessible path supplied in a results file.
- No test project is currently present. FluentValidation is not referenced or wired.

## Overview Maintenance

Use this file alone for normal project orientation. For implementation or debugging,
read only the relevant source/tests needed to verify behavior. Update this overview
alongside changes to responsibilities, DI, configuration, file contracts, or risks;
it is a maintained snapshot, not an automatically enforced source of truth.