# Task 005 — Register `IImageDisplayManager` in Desktop DI

Goal

- Make the `IImageDisplayManager` implementation available to the MAUI UI via dependency injection.

Deliverables

- Updated `src/ImageDuplicationSearcher.Desktop/MauiProgram.cs`

Description

- Add a singleton registration for `IImageDisplayManager` (implementation from Application.Services).
- Ensure Desktop project references the Application assembly project if not already set.

Acceptance Criteria

- Application builds and `MainPage` can receive an `IImageDisplayManager` instance via constructor injection.

Estimated time

- 15–30 minutes

Dependencies

- Task 001 and Task 003 (interface + implementation)
