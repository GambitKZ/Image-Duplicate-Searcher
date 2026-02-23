# Task 001 — Inspect MAUI project and add DI registration

Goal
Inspect the existing `ImageDuplicationSearcher.Desktop` MAUI project and add a call to the application's service registration so the Desktop host reuses the Console DI setup.

Deliverable
- Modify `MauiProgram.cs` to call the same `ConfigureServices` / `ServiceConfiguration` extension used by the Console app.
- Confirm `WorkflowService`, `IImageProcessor`, and `IReporter` resolve from DI.

Estimated time
30–60 minutes

Dependencies
- None
