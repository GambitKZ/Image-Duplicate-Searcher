# Maui Registration Notes — Task 003

Date: 2026-04-10

Summary
- Verified dependency injection registrations in `src/ImageDuplicationSearcher.Desktop/MauiProgram.cs`.

Findings
- `IResultsLoader` registered as Singleton (`ResultsLoader`).
- `IDuplicateNavigator` registered as Singleton (`DuplicateNavigator`).
- `IImageDisplayManager` registered as Singleton (`ImageDisplayManager`).
- `IImageRemovalService` registered as Singleton (`ImageRemovalService`).
- `MainPage` and `AppShell` registered as `Transient` — appropriate for view types.
- `ImageDuplicationOptions` are configured via `builder.Services.Configure<ImageDuplicationOptions>(...)`.
- No `WorkflowService`, `ImageProcessor`, or `Reporter` registrations are present in the Desktop `MauiProgram` — this matches FR-005 design.

Conclusion
- DI registration in `MauiProgram.cs` matches FR-005 requirements: business logic services live in the Application layer and are registered as Singletons for the Desktop app. No changes required.

Recommendations
- None required for registrations. If adapter-based file-picking is introduced (`IPlatformFileService`), register it here as `Transient` or `Singleton` depending on implementation; prefer `Transient` if it captures platform-specific state during picking.
