# Task 003 — Verify Maui Service Registration

Summary
- Validate `MauiProgram.cs` registers Application services as Singletons and that search/workflow services are not registered in the Desktop app.

Goal
- Ensure DI registration matches design: `IResultsLoader`, `IDuplicateNavigator`, `IImageDisplayManager`, `IImageRemovalService` registered as Singletons; no `WorkflowService`/`ImageProcessor`/`Reporter` registrations in Desktop.

Deliverable
- PR that updates `MauiProgram.cs` if registration is missing or incorrect.

Estimate
- 0.5 hour

Dependencies
- Task 001
