# REQ-20260222-001:FR-004 — DI and Configuration Integration

# Design Considerations
- Reuse the existing `ConfigureServices` extension (found in `ImageDuplicateSearcher.Application/Extensions/ConfigureServices.cs`) in `MauiProgram.cs`.
- Load `appsettings.json` where supported; treat it as defaults only. The UI should supply an in-memory `ImageDuplicationOptions` instance that the workflow consumes.
- Use transient or scoped services as per existing console app registrations to maintain parity.

# Data Flow
1. `MauiProgram` builds configuration and registers services.
2. The UI constructs or mutates an `ImageDuplicationOptions` object and passes it to `WorkflowService` at invocation time.

# Affected Components
- `MauiProgram.cs`
- `ConfigureServices` extension
- `ImageDuplicationOptions`

# Implementation Steps
1. Update `MauiProgram.cs` to call the existing service registration method.
2. Expose a way for the UI to obtain and override `ImageDuplicationOptions` (e.g., via `IOptionsMonitor<ImageDuplicationOptions>` or by passing an explicit options instance to the workflow invocation).
