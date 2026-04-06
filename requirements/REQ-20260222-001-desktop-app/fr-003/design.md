# REQ-20260222-001:FR-003 — UI: Start Scan and Background Execution

# Design Considerations
- Use `Task.Run` or an async method on the `WorkflowService` to execute the scan off the UI thread.
- Use `IProgress<string>` or an event-based `IReporter` to push status messages to the UI.
- Ensure cancellation support via `CancellationToken` so users can stop long-running scans.

# Data Flow
1. `Start Scan` handler validates inputs and creates a `CancellationTokenSource`.
2. Handler invokes `WorkflowService.ExecuteWorkflowAsync(options, progress, token)`.
3. `Reporter` reports progress via `IProgress<string>` or events; UI appends messages to the Response area.

# Affected Components
- `WorkflowService` (add an async entry point if missing)
- `IReporter` / UI-aware reporter
- `MainPage.xaml.cs` (start/cancel handlers)

# Implementation Steps
1. Add `ExecuteWorkflowAsync(ImageDuplicationOptions, IProgress<string>, CancellationToken)` to `WorkflowService` or add a wrapper that runs the existing workflow on a background task.
2. Implement cancellation and progress forwarding in `WorkflowService` and `Reporter`.
3. Wire UI `Start`/`Cancel` buttons to create/trigger the background work.
