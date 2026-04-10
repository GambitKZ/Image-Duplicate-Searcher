# MainPage Refactor Notes

Date: 2026-04-10

Summary
- Refactored `MainPage` to remove direct `FilePicker` usage and instead use the `IPlatformFileService` adapter. This isolates platform-dependent file-picking and permission logic from the UI layer and improves testability.

Files changed
- `src/ImageDuplicationSearcher.Desktop/MainPage.xaml.cs` — constructor now accepts `IPlatformFileService`; `OnOpenJsonResults` uses `EnsureReadPermissionAsync` and `PickJsonFileAsync`.
- `src/ImageDuplicationSearcher.Desktop/Services/IPlatformFileService.cs` — interface for platform file operations.
- `src/ImageDuplicationSearcher.Desktop/Services/WindowsPlatformFileService.cs` — Windows implementation (registered in `MauiProgram.cs`).
- `src/ImageDuplicationSearcher.Desktop/Services/AndroidPlatformFileService.cs` — Android implementation (registered conditionally via `#if ANDROID`).

Manual verification
- Launch the MAUI app on Windows; invoke "Open JSON Results" and confirm the file picker appears and a valid JSON file loads into the UI.
- On Android, allow permission prompts when requested and verify the picker flow and cache-copy fallback behave as described in `android-adapter-notes.md`.
