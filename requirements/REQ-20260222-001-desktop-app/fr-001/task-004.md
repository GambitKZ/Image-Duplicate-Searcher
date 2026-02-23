# Task 004 — Add UI-aware `IReporter` implementation

Goal
Implement `UiReporter` that implements `IReporter` and exposes an `OnLogMessage` event for the UI to subscribe to, while delegating JSON output to the existing persistence logic.

Deliverable
- `UiReporter.cs` file in `ImageDuplicateSearcher.Application` or Desktop project.
- Unit/Manual test steps showing UI subscription and file output.

Estimated time
1–2 hours

Dependencies
- Task 001
