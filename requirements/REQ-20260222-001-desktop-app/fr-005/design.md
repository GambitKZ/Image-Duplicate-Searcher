# REQ-20260222-001:FR-005 — UI-aware Reporter

# Design Considerations
- Implement a new `UiReporter` that implements `IReporter` and exposes an event `OnLogMessage(string)` or an `IObservable<string>`.
- The `UiReporter` SHOULD delegate JSON file writing to the existing `Reporter` logic or reuse the same persistence helper so file output behavior remains identical.
- Keep the existing Console `Reporter` intact and register `UiReporter` in the MAUI host; consider registering both and selecting the UI reporter when running from the Desktop app.

# Data Flow
1. `WorkflowService` calls `IReporter.ReportStatus()` / `ReportDuplicates()` as before.
2. `UiReporter` converts status messages into UI events; the UI subscribes and appends messages to the Response area.
3. `UiReporter` calls the shared persistence logic to write the JSON output.

# Affected Components
- `IReporter` interface
- `Reporter` implementation
- New `UiReporter` adapter
- `MauiProgram.cs` service registration

# Implementation Steps
1. Add `UiReporter` that implements `IReporter` and exposes `event Action<string> OnLogMessage`.
2. In `MauiProgram.cs`, register `UiReporter` as the `IReporter` implementation for the Desktop host.
3. Update the UI to subscribe to `OnLogMessage` and append messages to the Response area.
