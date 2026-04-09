# Task 004 — Register `IImageRemovalService` in Desktop DI (`MauiProgram.cs`)

Goal
- Ensure Desktop app has the removal service available via DI so `MainPage` and `ImageTileViewModel` can consume it.

Deliverable
- Edit `src/ImageDuplicationSearcher.Desktop/MauiProgram.cs` to register the implementation:
  - `builder.Services.AddSingleton<IImageRemovalService, ImageRemovalService>();`

Acceptance Criteria
- New registration compiles and the service can be resolved by the Desktop project.

Estimated Effort
- 15–30 minutes

Dependencies
- Task 003: `ImageRemovalService` implementation

Notes
- Desktop currently registers other Application services directly in `MauiProgram.cs`; follow the same pattern and lifetime (singleton).