# REQ-20260222-001:FR-001 — MAUI Desktop Host

# Design Considerations
- Create or reuse the `ImageDuplicationSearcher.Desktop` MAUI project as the Desktop host.
- `MauiProgram.cs` MUST register the same DI services as the Console application using the existing `ServiceConfiguration` extension method to ensure consistent behavior.
- Bind `ImageDuplicationOptions` from `appsettings.json` as defaults; allow UI to provide overrides at runtime.

# Data Flow
1. App startup loads configuration from `appsettings.json` and registers services.
2. The UI constructs or updates `ImageDuplicationOptions` and invokes the `WorkflowService`.

# Affected Components
- `ImageDuplicationSearcher.Desktop` project
- `MauiProgram.cs`
- `ServiceConfiguration` DI extension

# Implementation Steps
1. Add service registration in `MauiProgram.cs` to call the same `ConfigureServices` extension used by the Console app.
2. Add configuration file loading (optional) to the MAUI host; use `appsettings.json` where supported.
3. Ensure `WorkflowService` can be resolved and invoked from the UI.
