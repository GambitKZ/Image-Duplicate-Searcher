# ImageDuplicateSearcher.Console

.NET 10 / C# 14 command-line host that scans a configured image directory and
writes duplicate candidates to a local JSON file for review in Desktop.

## Summary

- Creates a Generic Host, loads configuration, and resolves WorkflowService.
- Runs one synchronous scan with terminal progress and duplicate-group output.
- Delegates hashing and reporting to the Application library; does not delete files.
- Has no network listener, background host loop, or interactive command menu.

## Components

COMPONENT_NAME: Program
TYPE: Entry Point / Composition Root
PURPOSE: Configure and execute a single duplicate scan.
RESPONSIBILITIES:
  - Call Host.CreateApplicationBuilder(args).
  - Add required appsettings.json from the process working directory, with reload
    enabled, then add user secrets for Program.
  - Call the Application ConfigureServices extension and build/dispose the host.
  - Resolve WorkflowService and call ExecuteWorkflow; does not call host.Run.
SOURCE: [Program.cs](Program.cs)
CALLS: Microsoft.Extensions.Hosting, configuration providers, DI, WorkflowService
CALLED_BY: dotnet run or the compiled executable

COMPONENT_NAME: Application Scan Pipeline
TYPE: Referenced Library Workflow
PURPOSE: Discover images, hash them, report duplicate candidates, and save JSON.
RESPONSIBILITIES:
  - Scan only top-level files matching SupportedFormats.
  - Use 32x32 JPEG thumbnails and exact equality of 64-bit perceptual hashes.
  - Print progress and groups via Spectre.Console; save only multi-image groups.
SOURCE: [../ImageDuplicateSearcher.Application/Services/WorkflowService.cs](../ImageDuplicateSearcher.Application/Services/WorkflowService.cs), [../ImageDuplicateSearcher.Application/Services/Reporter.cs](../ImageDuplicateSearcher.Application/Services/Reporter.cs)
CALLS: IImageProcessor, IReporter, filesystem, ImageSharp
CALLED_BY: Program

## Dependency Registration and Wiring

REGISTRATION_FILE: [Program.cs](Program.cs) delegates to
[../ImageDuplicateSearcher.Application/Extensions/ConfigureServices.cs](../ImageDuplicateSearcher.Application/Extensions/ConfigureServices.cs).

| Lifetime | Service | Implementation |
| --- | --- | --- |
| Transient | WorkflowService | WorkflowService |
| Transient | IImageProcessor | ImageProcessor |
| Transient | IReporter | Reporter |

The extension also binds IOptions<ImageDuplicationOptions> from the named section.
The host supplies configuration and standard hosting infrastructure. UiReporter
and Desktop review services are not registered here.

## Communication Channels

- Input: process arguments through Generic Host configuration, configuration
  providers, and image files on the local filesystem.
- Output: Spectre.Console terminal progress/group listings and a local JSON file.
- Desktop communication is file-based only, not IPC or an API. The Desktop user
  selects the exported file manually; the apps do not start each other.
- JSON shape: `[{"hash":123,"images":[{"path":"C:/images/a.jpg","sizeMB":1.5}]}]`.
  hash is an unsigned 64-bit number; sizeMB is bytes / 1024 / 1024. The reporter
  emits only groups of two or more images. Paths come from directory enumeration,
  so relative scan paths may produce relative image paths in the export.

## Configuration Keys

Values shipped in [appsettings.json](appsettings.json):

| Key | Shipped Value | Purpose |
| --- | --- | --- |
| ImageDuplicationOptions:ImageDirectory | ./images | Directory to scan, nonrecursive |
| ImageDuplicationOptions:OutputFilePath | duplicates.json | JSON output destination |
| ImageDuplicationOptions:SupportedFormats | .jpeg, .jpg, .png, .bmp | Allowed image extensions |

Configuration precedence is important: Host.CreateApplicationBuilder first adds
its default providers (including environment variables and command-line arguments).
Program then appends appsettings.json and user secrets, so these later providers
override matching keys from those defaults. Do not assume arguments or environment
variables take precedence over this explicitly appended file.

Paths resolve relative to the process working directory. appsettings.json is copied
to build output with PreserveNewest, but Program still looks in the working
directory. A UserSecretsId is configured in the project; secrets are optional local
configuration, not checked-in settings. Although JSON reload is enabled, services
consume IOptions rather than IOptionsMonitor and execute only once.

## Running

Requires the .NET 10 SDK and an existing configured image directory. From this
project directory, run `dotnet run`. Ensure the output parent directory exists and
is writable. No MAUI workload is needed to run this project independently.

## Known Risks

- Missing configuration, a nonexistent input directory, corrupt images, or output
  access failures can terminate the process; Program has no top-level error mapping
  or custom exit-code policy. No options validation is registered.
- Shipped ./images is not created by the application. Working-directory differences
  affect settings discovery, scan paths, output location, and later Desktop reads.
- A no-duplicates scan does not clear or rewrite an existing duplicates.json;
  stale results can appear to belong to the latest scan.
- JSON writes overwrite non-atomically and contain filesystem paths, which can
  expose local directory information when shared.
- Hash equality does not prove identical content. Scanning is serial, nonrecursive,
  and aborts on an unreadable image rather than skipping it.
- Program uses the synchronous wrapper without cancellation integration. The host
  is built but not started, so this is not a long-running hosted service.
- No test project is currently present.

## Overview Maintenance

Use this file alone for normal project orientation. For implementation or debugging,
read only the relevant source/tests needed to verify behavior. Update this overview
when startup, DI, configuration precedence, output contracts, or failure behavior
changes. No agent instruction currently enforces overview-only reading.