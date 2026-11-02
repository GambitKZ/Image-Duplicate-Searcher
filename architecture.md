# ImageDuplicateSearcher
Dual .NET 10 application for identifying and managing duplicate images via perceptual hashing.
Two-tier: Console search engine writes JSON results; Desktop MAUI app reads results and manages image deletion.

## Technology Stack

RUNTIME: .NET 10.0.100, C# 14
CONSOLE_FRAMEWORK: Console app with HostApplicationBuilder, dependency injection
DESKTOP_FRAMEWORK: MAUI (cross-platform Windows/Android UI)
IMAGE_PROCESSING: SixLabors.ImageSharp (thumbnail generation, validation, rendering)
HASHING_ALGORITHM: Discrete Cosine Transform (DCT) for perceptual hashing
CONSOLE_UI: Spectre.Console (rich console output)
SERIALIZATION: System.Text.Json (JSON results)
DEPENDENCY_INJECTION: Microsoft.Extensions.DependencyInjection
CONFIGURATION: Microsoft.Extensions.Configuration (appsettings.json, user secrets)

## Services (Console & Desktop)

### WorkflowService
TYPE: Orchestration Service
OWNS_RESPONSIBILITY: Main workflow; scans images, computes hashes, groups duplicates
ENTRY_POINT: src/ImageDuplicateSearcher.Console/Program.cs
IMPLEMENTS: Runs as top-level service in HostApplicationBuilder
DEPENDS_ON: IImageProcessor, IUiReporter, IOptions<ImageDuplicationOptions>
PLATFORM: Console only
METHOD_SIGNATURES:
  ExecuteWorkflow() -> void (loops through images, calls ImageProcessor, groups by hash, calls Reporter)

### ImageProcessor
TYPE: Image Processing Service
OWNS_RESPONSIBILITY: Scan images, generate thumbnails, compute perceptual hashes
LOCATION: src/ImageDuplicateSearcher.Application/Services/ImageProcessor.cs
IMPLEMENTS: IImageProcessor
ENTRY_POINT: Injected into WorkflowService
DEPENDS_ON: SixLabors.ImageSharp, System.IO, IOptions<ImageDuplicationOptions>
PLATFORM: Console only
READS: File system (configured image directory)
OUTPUTS: In-memory hash dictionary (ulong -> List<ImageModel>)
METHOD_SIGNATURES:
  GetImageList() -> ImageModel[]
  GenerateThumbnail(imagePath: string) -> byte[] (32x32 JPEG)
  ComputePerceptualHash(thumbnail: byte[]) -> ulong
CONFIGURATION_FIELDS: ImageDirectory, SupportedFormats

### Reporter
TYPE: Results Output Service
OWNS_RESPONSIBILITY: Display duplicates to console, persist results to JSON file
LOCATION: src/ImageDuplicateSearcher.Application/Services/Reporter.cs
IMPLEMENTS: IUiReporter
ENTRY_POINT: Injected into WorkflowService
DEPENDS_ON: Spectre.Console, System.Text.Json, System.IO
PLATFORM: Console only
READS: Memory (grouped duplicate dictionary from WorkflowService)
WRITES: Console (stdout), File system (JSON)
METHOD_SIGNATURES:
  ReportDuplicates(duplicateGroups: Dictionary<ulong, List<ImageModel>>) -> void
  SaveResultsToJson(duplicateGroups, outputPath: string) -> void
OUTPUT_FILE_FORMAT: JSON array of DuplicateSearchResult objects
OUTPUT_LOCATION: ImageDuplicationOptions.OutputFilePath

### ResultsLoader
TYPE: Data Loading Service
OWNS_RESPONSIBILITY: Deserialize and load JSON results from console output
LOCATION: src/ImageDuplicateSearcher.Application/Services/ResultsLoader.cs
IMPLEMENTS: IResultsLoader
ENTRY_POINT: Injected into Desktop MAUI services
DEPENDS_ON: System.Text.Json, System.IO
PLATFORM: Desktop (MAUI) only
READS: File system (JSON results file)
OUTPUTS: Memory (DuplicateSearchResult[] objects)
METHOD_SIGNATURES:
  LoadResultsAsync(jsonFilePath: string) -> Task<DuplicateSearchResult[]>
  GetResultByHashAsync(hash: ulong) -> Task<DuplicateSearchResult>
ERROR_HANDLING: Throws meaningful exceptions on malformed JSON or file access errors

### DuplicateNavigator
TYPE: Navigation & State Service
OWNS_RESPONSIBILITY: Track current position in hash groups, provide navigation controls
LOCATION: src/ImageDuplicateSearcher.Application/Services/DuplicateNavigator.cs
IMPLEMENTS: IDuplicateNavigator
ENTRY_POINT: Injected into Desktop MAUI services
PLATFORM: Desktop (MAUI) only
LIFETIME: Singleton (maintains state for entire app session)
STATE_HELD: CurrentGroupIndex (int), AllGroups (DuplicateSearchResult[])
METHOD_SIGNATURES:
  Initialize(results: DuplicateSearchResult[]) -> void (reset to index 0)
  NextGroup() -> void
  PreviousGroup() -> void
  GoToGroup(groupIndex: int) -> void (validate bounds)
  GetCurrentGroup() -> DuplicateSearchResult?
  GetCurrentGroupIndex() -> int (0-based)
  GetTotalGroupCount() -> int
  CanGoNext() -> bool
  CanGoPrevious() -> bool

### ImageDisplayManager
TYPE: UI Rendering Service
OWNS_RESPONSIBILITY: Load images for display, render placeholders for missing/corrupted files
LOCATION: src/ImageDuplicateSearcher.Application/Services/ImageDisplayManager.cs
IMPLEMENTS: IImageDisplayManager
ENTRY_POINT: Injected into Desktop MAUI services
DEPENDS_ON: SixLabors.ImageSharp, System.IO, MAUI (ImageSource API)
PLATFORM: Desktop (MAUI) only
READS: File system (image files)
OUTPUTS: Memory (MAUI ImageSource objects)
METHOD_SIGNATURES:
  GetImageSourceAsync(imagePath: string) -> Task<ImageSource> (returns placeholder on error)
  GetImageBytesAsync(imagePath: string) -> Task<byte[]>
  GetPlaceholderImage() -> ImageSource
  IsImageAccessible(imagePath: string) -> bool
ERROR_HANDLING: Graceful fallback to placeholder on FileNotFound, IOException, UnauthorizedAccessException
CACHE_STRATEGY: In-memory cache to reduce repeated file I/O

### ImageRemovalService
TYPE: File Operations Service
OWNS_RESPONSIBILITY: Delete image files from disk (user-initiated removal)
LOCATION: src/ImageDuplicateSearcher.Application/Services/ImageRemovalService.cs
IMPLEMENTS: IImageRemovalService
ENTRY_POINT: Injected into Desktop MAUI services
DEPENDS_ON: System.IO
PLATFORM: Desktop (MAUI) only
WRITES: File system (deletion only)
METHOD_SIGNATURES:
  RemoveImageAsync(imagePath: string) -> Task<bool> (true if success, false if not found/permission denied)
  RemoveImagesAsync(imagePaths: string[]) -> Task<(int Successful, int Failed)> (bulk delete)
  ValidateImagePath(imagePath: string) -> string? (returns null if valid, error message if invalid)
SECURITY: Validates paths to prevent directory traversal attacks
ERROR_HANDLING: Catches and returns status; does not throw exceptions for business failures

## Data Models & Configuration

### ImageModel
LOCATION: src/ImageDuplicateSearcher.Application/Models/ImageModel.cs
PURPOSE: Represents individual image metadata
FIELDS: FilePath (string), SizeBytes (long)
USAGE: Grouped by hash (Dictionary<ulong, List<ImageModel>>) in console; persisted and loaded in desktop

### DuplicateSearchResult
LOCATION: src/ImageDuplicateSearcher.Application/Models/DuplicateSearchResult.cs
PURPOSE: JSON-serializable result for one hash group
FIELDS: Hash (ulong), Images (DuplicateSearchResultImage[])
SERIALIZATION_FORMAT:
  {
    "hash": "18446744073709551615",
    "images": [
      { "path": "C:\\Users\\...\\image1.jpg", "sizeMB": 2.5 },
      { "path": "C:\\Users\\...\\image2.jpg", "sizeMB": 2.5 }
    ]
  }

### DuplicateSearchResultImage
LOCATION: src/ImageDuplicateSearcher.Application/Models/DuplicateSearchResultImage.cs
PURPOSE: Individual image entry in a duplicate group
FIELDS: Path (string), SizeMB (double)

### ImageDisplayResult
LOCATION: src/ImageDuplicateSearcher.Application/Models/ImageDisplayResult.cs
PURPOSE: UI-friendly result for desktop display
FIELDS: FilePath (string), SizeDisplayText (string), IsAccessible (bool)

### RemovalResult
LOCATION: src/ImageDuplicateSearcher.Application/Models/RemovalResult.cs
PURPOSE: Result of image removal operation
FIELDS: Success (bool), ErrorMessage (string?)

### ImageDuplicationOptions
LOCATION: src/ImageDuplicateSearcher.Application/Settings/ImageDuplicationOptions.cs
CONFIG_SECTION: "ImageDuplication" in appsettings.json
PLATFORM: Console only (desktop reads pre-generated results, not configuration)
FIELDS:
  ImageDirectory: string (directory to scan for images)
  OutputFilePath: string (where console writes JSON results)
  SupportedFormats: string[] (file extensions: .jpg, .png, .bmp, .gif, .webp)
  ThumbnailSize: int (pixels, default 32)
BINDING: via IOptions<ImageDuplicationOptions> in DI

## Communication Map

### INTERACTION 1: Console Initialization
  FROM: Program.cs (Program.Main)
  TO: HostApplicationBuilder, ConfigurationBuilder, Service Registration
  PROTOCOL: In-process (DI container)
  PATTERN: Initialization
  PURPOSE: Build host, register services, load configuration

### INTERACTION 2: WorkflowService → ImageProcessor
  FROM: WorkflowService.ExecuteWorkflow()
  TO: IImageProcessor (interface)
  PROTOCOL: In-process (synchronous method calls)
  PATTERN: Request-Response
  DIRECTION: Caller → Callee
  PURPOSE: Scan images and compute hashes
  CALLS:
    GetImageList() -> ImageModel[]
    For each image: GenerateThumbnail(path) -> byte[]
    For each thumbnail: ComputePerceptualHash(bytes) -> ulong

### INTERACTION 3: WorkflowService → Reporter
  FROM: WorkflowService.ExecuteWorkflow()
  TO: IUiReporter (interface)
  PROTOCOL: In-process (synchronous method calls)
  PATTERN: Request-Response
  DIRECTION: Caller → Callee
  PURPOSE: Output duplicate groups to console and JSON file
  CALLS:
    ReportDuplicates(Dictionary<ulong, List<ImageModel>>) -> void

### INTERACTION 4: Reporter → File System (JSON Persistence)
  FROM: Reporter.SaveResultsToJson()
  TO: Disk (file system)
  PROTOCOL: .NET File I/O (System.IO.File)
  PATTERN: Write
  DIRECTION: Reporter → Disk
  ENCODING: UTF-8 JSON
  PURPOSE: Persist duplicate groups for desktop app to load later
  OUTPUT_PATH: ImageDuplicationOptions.OutputFilePath

### INTERACTION 5: Desktop Initialization
  FROM: MauiProgram.cs (CreateMauiApp)
  TO: Service Registration (DI container), Platform Setup
  PROTOCOL: In-process (DI container)
  PATTERN: Initialization
  PURPOSE: Register desktop services: IResultsLoader, IDuplicateNavigator, IImageDisplayManager, IImageRemovalService
  EXCLUDES: WorkflowService, ImageProcessor, Reporter (console-only)

### INTERACTION 6: Desktop UI (File Picker) → File System (JSON Load)
  FROM: MainPage.xaml.cs (OnOpenResultsClicked)
  TO: File system (via MAUI FilePicker API)
  PROTOCOL: Platform-specific file picker (Windows API, Android storage)
  PATTERN: File Selection
  PURPOSE: User selects JSON results file from previous console run

### INTERACTION 7: Desktop UI → ResultsLoader
  FROM: MainPage (after file selected)
  TO: IResultsLoader (interface)
  PROTOCOL: In-process (async Task-based)
  PATTERN: Request-Response (Async)
  DIRECTION: UI → Service
  PURPOSE: Load and deserialize JSON results
  CALLS:
    LoadResultsAsync(filePath: string) -> Task<DuplicateSearchResult[]>
  ERROR_HANDLING: Catch exceptions; display user-friendly error messages

### INTERACTION 8: Desktop UI (Summary) ← DuplicateNavigator
  FROM: MainPage UI bindings (CurrentGroupIndex, TotalGroupCount)
  TO: IDuplicateNavigator (interface)
  PROTOCOL: In-process (property getters, property-changed events)
  PATTERN: State Query, Binding
  PURPOSE: Display "Current Group: X / Y" summary

### INTERACTION 9: Desktop UI (Navigation Buttons) → DuplicateNavigator
  FROM: MainPage navigation buttons (Next, Previous, Jump)
  TO: IDuplicateNavigator (interface)
  PROTOCOL: In-process (method calls)
  PATTERN: Command
  DIRECTION: UI → Navigator
  PURPOSE: Move to next/previous group or jump to specific index
  CALLS:
    NextGroup() / PreviousGroup() / GoToGroup(index)
    GetCurrentGroup() -> DuplicateSearchResult?
  VALIDATION: Navigator checks bounds before moving

### INTERACTION 10: Desktop UI (Image Canvas) → ImageDisplayManager
  FROM: MainPage image rendering (for each image in current group)
  TO: IImageDisplayManager (interface)
  PROTOCOL: In-process (async Task-based)
  PATTERN: Request-Response (Async)
  DIRECTION: UI → Service
  PURPOSE: Load image thumbnails for display
  CALLS:
    GetImageSourceAsync(imagePath: string) -> Task<ImageSource>
  ERROR_HANDLING: Returns placeholder ImageSource if file missing/corrupted
  OPTIMIZATION: Caches loaded images in memory

### INTERACTION 11: Desktop UI (Remove Button) → ImageRemovalService → File System
  FROM: MainPage remove button (for selected image)
  TO: IImageRemovalService (interface)
  PROTOCOL: In-process (async Task-based)
  PATTERN: Command, State Mutation
  DIRECTION: UI → Service → File System
  PURPOSE: Delete image file from disk
  CALLS:
    RemoveImageAsync(imagePath: string) -> Task<bool>
  SIDE_EFFECT: Deletes file; UI refreshes current group view
  ERROR_HANDLING: Catches IO exceptions; displays error message to user instead of crashing

### INTERACTION 12: ImageRemovalService → File System (Deletion)
  FROM: ImageRemovalService.RemoveImageAsync()
  TO: Disk (file system, System.IO.File.Delete)
  PROTOCOL: .NET File I/O
  PATTERN: Delete
  DIRECTION: Service → Disk
  PURPOSE: Remove image file selected by user
  CONCURRENCY: May throw if file in use (handled gracefully)

## Ownership & Responsibilities

### ImageDuplicateSearcher.Application (Shared Business Logic)
  OWNS:
    - IImageProcessor, IUiReporter (console interfaces)
    - IResultsLoader, IDuplicateNavigator, IImageDisplayManager, IImageRemovalService (desktop interfaces)
    - All Models: ImageModel, DuplicateSearchResult, DuplicateSearchResultImage, ImageDisplayResult, RemovalResult
    - Configuration: ImageDuplicationOptions
  PROVIDES_TO: Console app and Desktop app
  TYPE: Class library (.csproj)

### ImageDuplicateSearcher.Console (Search Engine)
  OWNS:
    - Program.cs (entry point)
    - appsettings.json (console configuration)
    - Console-specific service implementations (if any: WorkflowService, Reporter, ImageProcessor)
  IMPLEMENTS: Searches images, computes hashes, groups duplicates, outputs results
  OUTPUT: JSON file (DuplicateSearchResult[])
  DOES_NOT_IMPLEMENT: Image viewing, image deletion (desktop responsibility)
  TYPE: Console app (.csproj)

### ImageDuplicationSearcher.Desktop (Results Browser & Manager)
  OWNS:
    - MauiProgram.cs (MAUI entry point)
    - MainPage.xaml / MainPage.xaml.cs (MAUI UI)
    - Platform-specific code (Platforms/ folder: Android, Windows implementations)
    - Desktop-specific service implementations (if any: ResultsLoader, DuplicateNavigator, ImageDisplayManager, ImageRemovalService)
  IMPLEMENTS: Loads JSON results, navigates groups, displays images, deletes images
  INPUT: JSON file (from console output)
  DOES_NOT_IMPLEMENT: Image hashing, duplicate detection (console responsibility)
  TYPE: MAUI app (.csproj)
  PLATFORMS: Windows, Android

## File I/O Summary

READS:
  - File system: Configured image directory (Console → ImageProcessor)
  - File system: JSON results file (Desktop → ResultsLoader)
  - File system: Image files for display (Desktop → ImageDisplayManager)

WRITES:
  - File system: JSON results file (Console → Reporter)
  - File system: Image file deletion (Desktop → ImageRemovalService)

DOES_NOT_WRITE:
  - JSON results file (never modified by desktop; console is sole writer)

## Data Persistence Format (JSON)

FILE_NAME: Specified by ImageDuplicationOptions.OutputFilePath (e.g., "duplicates.json")
LOCATION: Disk (filesystem)
FORMAT: JSON array of DuplicateSearchResult objects
ENCODING: UTF-8
EXAMPLE:
  [
    {
      "hash": "18446744073709551615",
      "images": [
        { "path": "C:\\Users\\john\\Pictures\\photo1.jpg", "sizeMB": 2.5 },
        { "path": "C:\\Users\\john\\Pictures\\photo2.jpg", "sizeMB": 2.5 }
      ]
    },
    {
      "hash": "9223372036854775807",
      "images": [
        { "path": "C:\\Users\\john\\Pictures\\vacation1.png", "sizeMB": 1.2 },
        { "path": "C:\\Users\\john\\Pictures\\vacation2.png", "sizeMB": 1.2 },
        { "path": "C:\\Users\\john\\Pictures\\vacation3.png", "sizeMB": 1.2 }
      ]
    }
  ]

LIFETIME: Written once by console; persisted indefinitely; read by desktop app(s); never modified</content>
<parameter name="filePath">l:\Development\StudioProject\ImageDuplicateSearcher\.spec-workflow\architecture.md