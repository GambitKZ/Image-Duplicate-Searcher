# Task 003 — Register `DuplicateNavigator` in Desktop DI

Goal
- Ensure the Desktop app resolves `IDuplicateNavigator` from DI so the UI can consume the centralized navigation state.

Deliverable
- Patch `src/ImageDuplicateSearcher.Desktop/MauiProgram.cs` to register the service, e.g.:
  - `builder.Services.AddSingleton<IDuplicateNavigator, DuplicateNavigator>();`

Acceptance criteria
- The application builds and `MainPage` can accept an `IDuplicateNavigator` through constructor injection.

Estimate
- 15–30 minutes

Notes
- If the project prefers centralizing registrations in a shared ConfigureServices extension, add the registration there and call it from `MauiProgram`.
