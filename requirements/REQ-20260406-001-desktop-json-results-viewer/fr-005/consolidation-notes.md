# Consolidation Notes — Task 002

Date: 2026-04-10

Summary
- Reviewed MAUI UI and view-models for business-rule leakage that should be moved into the Application layer. No duplicated business rules were found; the UI delegates load, navigation, rendering, and removal to Application services as intended.

Findings
- `MainPage.xaml.cs` delegates JSON loading to `IResultsLoader`, navigation to `IDuplicateNavigator`, image loading to `IImageDisplayManager`, and removal to `IImageRemovalService` — appropriate separation.
- `ImageTileViewModel` delegates image validation/loading and deletion to Application services and only maps results to UI-friendly properties — correct.

Observations / Recommended Consolidations
- Platform picker: `MainPage` calls `FilePicker.PickAsync` directly. This is a platform interaction (file picking & permissions) rather than business logic, but to improve testability and isolate platform concerns, introduce an `IPlatformFileService` and move file-picking and runtime permissions behind it (Task 005/006/007).
- `ResultsLoader` exception behavior: it wraps `JsonException` into an `InvalidOperationException`. The interface documents `JsonException` and UI currently catches `JsonException` directly. Recommendation: either (A) let `JsonException` bubble from `ResultsLoader` (preferred), or (B) update interface/docs and UI to expect `InvalidOperationException`. This is a small alignment task and not a business-rule move.

Conclusion
- No business-rule consolidation was required beyond platform adapter work and the small `ResultsLoader` exception alignment. Task 002 is complete: Application-layer rules are correctly centralized; work remains on platform adapters and minor alignment tasks.
