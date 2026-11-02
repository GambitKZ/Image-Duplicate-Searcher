# ImageDuplicationSearcher.Desktop

.NET 10 / C# 14 MAUI application for opening duplicate-result JSON, navigating
image groups, previewing files, and removing selected images. Despite its name,
the project targets Android as well as Windows.

## Summary

- Reviews results produced by Console; does not currently expose scanning UI or
  register WorkflowService, IImageProcessor, IReporter, or UiReporter.
- Opens a user-selected JSON file, validates it through Application services,
  and shows groups in a three-column image grid.
- Supports previous/next and one-based group-number navigation.
- Removes selected files and hides successfully deleted images for the current
  session; never rewrites the loaded JSON.
- Targets net10.0-android and, on Windows build hosts,
  net10.0-windows10.0.19041.0. Minimum versions are Android 21 and Windows
  10.0.17763.0. MacCatalyst startup files exist but it is not an enabled target.

## Components

COMPONENT_NAME: MauiProgram / App / AppShell
TYPE: Composition Root / Window and Shell
PURPOSE: Configure MAUI and construct the injected page hierarchy.
RESPONSIBILITIES:
  - Enable MAUI Community Toolkit, configure options, and register dependencies.
  - App receives AppShell and places it in a Window.
  - AppShell receives MainPage and adds it as ShellContent.Content; the page is
    created by DI rather than parameterless XAML DataTemplate activation.
SOURCE: [MauiProgram.cs](MauiProgram.cs), [App.xaml.cs](App.xaml.cs), [AppShell.xaml.cs](AppShell.xaml.cs)
CALLS: MAUI builder, DI, MainPage
CALLED_BY: Platform startup

COMPONENT_NAME: MainPage
TYPE: UI / Code-Behind Coordinator
PURPOSE: Load result files and coordinate navigation and image-tile presentation.
RESPONSIBILITIES:
  - Request file access, open the picker, and load/validate selected JSON.
  - Initialize IDuplicateNavigator and react to its PropertyChanged events.
  - Rebuild an ObservableCollection of tiles, excluding IsDeleted images.
  - Marshal UI changes through MainThread; calculate image height from the first
    usable image's dimensions and collection width.
SOURCE: [MainPage.xaml](MainPage.xaml), [MainPage.xaml.cs](MainPage.xaml.cs)
CALLS: IResultsLoader, IDuplicateNavigator, IImageDisplayManager, IImageRemovalService, IPlatformFileService
CALLED_BY: AppShell, UI events, navigator notifications

COMPONENT_NAME: ImageTileViewModel
TYPE: Tile View Model
PURPOSE: Bind image data, placeholder state, dimensions, and removal commands.
RESPONSIBILITIES:
  - Convert ImageDisplayResult bytes to MAUI ImageSource on the UI thread.
  - Map RemovalResult outcomes into UI state/status callbacks.
  - On NotFound, attempt IPlatformFileService.TryDeleteOriginalAsync as fallback.
  - Mark DuplicateSearchResultImage.IsDeleted after success and refresh the page.
SOURCE: [ViewModels/ImageTileViewModel.cs](ViewModels/ImageTileViewModel.cs)
CALLS: IImageDisplayManager, IImageRemovalService, IPlatformFileService, MainThread
CALLED_BY: MainPage and tile RemoveCommand bindings

COMPONENT_NAME: Platform File Services
TYPE: OS Adapters
PURPOSE: Abstract permissions, result-file selection, and fallback deletion.
RESPONSIBILITIES:
  - WindowsPlatformFileService uses MAUI FilePicker without runtime permissions.
  - AndroidPlatformFileService requests StorageRead permission and accepts .json
    selections; copies picker streams to app cache when no filesystem path exists.
  - Return PlatformFileResult with Path, IsTemporaryCopy, and OriginalUri (currently
    null for picked files); Android fallback can attempt content:// deletion.
SOURCE: [Services/IPlatformFileService.cs](Services/IPlatformFileService.cs), [Services/WindowsPlatformFileService.cs](Services/WindowsPlatformFileService.cs), [Services/AndroidPlatformFileService.cs](Services/AndroidPlatformFileService.cs), [Services/PlatformFileResult.cs](Services/PlatformFileResult.cs)
CALLS: MAUI FilePicker/Permissions, filesystem, Android ContentResolver
CALLED_BY: MainPage and ImageTileViewModel

## Dependency Registration and Wiring

REGISTRATION_FILE: [MauiProgram.cs](MauiProgram.cs)

| Lifetime | Service | Implementation |
| --- | --- | --- |
| Singleton | IResultsLoader | ResultsLoader |
| Singleton | IDuplicateNavigator | DuplicateNavigator |
| Singleton | IImageDisplayManager | ImageDisplayManager |
| Singleton | IImageRemovalService | ImageRemovalService |
| Transient | IPlatformFileService | AndroidPlatformFileService under ANDROID; WindowsPlatformFileService otherwise |
| Transient | MainPage | MainPage |
| Transient | AppShell | AppShell |

The four singleton implementations live in the Application library. This host
does not call its ConfigureServices extension. MAUI supplies framework services
and App activation. CommunityToolkit.Maui is enabled; there is no explicit
application logging-provider setup despite a Logging.Debug package reference.

## Communication Channels

- Input: manually selected local JSON, then image paths referenced by its groups.
  There is no API, message broker, database, or live connection to Console.
- JSON contract: `[{"hash":123,"images":[{"path":"C:/images/a.jpg","sizeMB":1.5}]}]`.
  hash is numeric ulong, sizeMB is MiB, and field matching is case-insensitive.
  The loader rejects empty arrays, groups with no images, blank paths, and negative
  sizes; a single-image group is accepted. Missing required model fields fail loading.
- In-process channels: DI service calls, INotifyPropertyChanged, MAUI commands,
  an ObservableCollection of tiles, and status/refresh callbacks.
- MainThread dispatch is required for UI changes; navigator events are raised on
  the caller's thread, not automatically marshalled by the Application library.
- Removal changes the filesystem and session-only IsDeleted flags, never the source
  JSON. Reloading the file restores entries for removed images as missing files.

## Configuration Keys

There is no appsettings.json loading or user-secrets binding in this host.
MauiProgram configures IOptions<ImageDuplicationOptions> directly:

| Option | Value |
| --- | --- |
| ImageDirectory | Empty string |
| OutputFilePath | FileSystem.AppDataDirectory combined with duplicates.json |
| SupportedFormats | .jpeg, .jpg, .png, .bmp |

These are configured defaults, not user-editable settings or active scan controls;
the current review services do not consume them. ResultsFileEntry displays the
picked file path and is read-only. Source-generated XAML inflation is enabled;
WindowsPackageType is None (unpackaged).

## Build and Run

Requires the .NET 10 SDK, matching MAUI workloads, and platform toolchains. On a
Windows development machine, build this project with
`dotnet build -f net10.0-windows10.0.19041.0` and run with
`dotnet run -f net10.0-windows10.0.19041.0`. Android requires its SDK and a configured
device/emulator. These commands are relative to this project directory.

## Known Risks

- Remove has no confirmation or undo UI. Application removal uses the Windows
  Recycle Bin but permanently deletes on Android; platform fallback deletion is
  direct. Loaded JSON paths are not restricted to an allowed directory.
- ImageRemovalService calls File.Delete again after platform-specific deletion,
  leaving a race if another file appears at the same path.
- Desktop cannot make another machine's absolute paths accessible. Copying Console
  results to Android does not grant access to the referenced Windows image files.
- Android StorageRead checks can block the picker even where picker-granted access
  would suffice under scoped storage. Selected JSON may be copied to cache, but
  referenced images are not imported or granted access automatically.
- MainPage only uses PlatformFileResult.Path; temporary-copy metadata is not carried
  forward. Android OriginalUri is not populated by the picker, so fallback deletion
  cannot be assumed to target an original document URI.
- Each navigator update raises several notifications; MainPage starts an unawaited
  refresh for each. Refreshes can overlap and access tiles before queued UI updates
  run. There is no cancellation or stale-result guard during rapid navigation.
- ImageDisplayManager retains full-resolution image bytes and placeholders in an
  unbounded singleton cache, without invalidation. Large collections can consume
  substantial memory and show stale image state.
- After loading, the load/status/error section is hidden; subsequent removal status
  messages target labels in that hidden section (tile placeholder reasons remain).
- MainPage subscribes to the singleton navigator without unsubscribing; recreating
  transient pages can retain old pages and duplicate notifications.
- No test project is currently present; MAUI/platform behavior needs manual checks.

## Overview Maintenance

Use this file alone for normal project orientation. For implementation or debugging,
read only the relevant source/tests needed to verify behavior. Update it when page
flows, DI, platform adapters, configuration, or deletion behavior changes. It records
the current review-only application, not planned scanning features.