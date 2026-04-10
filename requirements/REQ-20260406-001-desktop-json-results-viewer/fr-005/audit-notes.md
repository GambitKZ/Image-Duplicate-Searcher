# Audit Notes — FR-005: Platform And Layer Boundaries

Date: 2026-04-10

Summary
- Application-layer services implement the expected business rules (load, navigate, render, remove) and the MAUI layer is primarily orchestration/presentation. A few small inconsistencies and platform gaps were identified and are listed below with recommended follow-ups.

Findings

1) Application services — generally compliant
- `src/ImageDuplicateSearcher.Application/Services/ResultsLoader.cs`
  - Loads and validates JSON, caches results, and returns typed models. Good separation of concerns.
  - Notable mismatch: the implementation catches `System.Text.Json.JsonException` and rethrows an `InvalidOperationException` (wrapping the original). The interface `IResultsLoader` documents `JsonException` as a possible exception and the UI currently has a dedicated `JsonException` catch block. Recommendation: make exception behavior consistent — either let `JsonException` bubble (preferred for distinct parsing error handling) or update the interface/docs and UI to expect `InvalidOperationException` for malformed JSON.
  - Minor: `_cachedResults` is not synchronized; if concurrent callers are possible, consider a simple lock or `Volatile`/`ImmutableArray` pattern.

- `src/ImageDuplicateSearcher.Application/Services/DuplicateNavigator.cs`
  - Correctly encapsulates navigation state, exposes `INotifyPropertyChanged`, and uses locks for thread-safety. Behavior aligns with design (UI uses display index conventions).

- `src/ImageDuplicateSearcher.Application/Services/ImageDisplayManager.cs`
  - Validates images with ImageSharp and returns deterministic placeholder bytes when needed. Good separation from UI types (returns `ImageDisplayResult`). Suggestion: consider cancellation token support for long-running loads and bounded cache size if large collections are expected.

- `src/ImageDuplicateSearcher.Application/Services/ImageRemovalService.cs`
  - Performs safe deletion and maps common filesystem failures to `RemovalResult`. Matches design intent to keep deletion logic in Application layer.
  - Design/implementation discrepancy: design.md described additional helpers (`ValidateImagePath`, bulk removal APIs). The current interface only exposes `RemoveImageAsync`. If bulk operations or path validation helpers are required by product, add them and implement accordingly.

2) UI layer — presentation / orchestration only (with one platform call)
- `src/ImageDuplicateSearcher.Desktop/MainPage.xaml.cs`
  - Uses `IResultsLoader`, `IDuplicateNavigator`, `IImageDisplayManager`, and `IImageRemovalService` for core rules — correct.
  - One platform concern remains in UI: `FilePicker.PickAsync` is invoked directly from `MainPage` (platform file-picking and permission concerns). Recommendation: introduce an `IPlatformFileService` and move file-picker and permission handling behind that adapter (see Task 005/006).

- `src/ImageDuplicateSearcher.Desktop/ViewModels/ImageTileViewModel.cs`
  - Proper view-model responsibilities: creates `ImageSource` for UI, delegates loading/removal to Application services, updates model `IsDeleted` flags. No business-rule leakage found.

3) DI / Registration
- `src/ImageDuplicateSearcher.Desktop/MauiProgram.cs` registers the Application services as singletons (IResultsLoader, IDuplicateNavigator, IImageDisplayManager, IImageRemovalService) and does not register search/workflow services for the Desktop app — this matches FR-005 design guidance.

4) Platform gaps — Android
- `src/ImageDuplicateSearcher.Desktop/Platforms/Android/AndroidManifest.xml` currently does not declare storage permissions (e.g., `READ_EXTERNAL_STORAGE` / `WRITE_EXTERNAL_STORAGE`) and does not include scoped-storage guidance. Android scoped-storage (API 29/30+) and SAF may limit direct file-path access; plan Tasks 004, 006, and 008 address these items: add manifest entries where appropriate, implement runtime permission requests or SAF flow in the Android adapter, and document limitations/fallbacks.

Recommended Next Actions
- Start Task 002: Consolidate Business Rules (move any remaining non-UI rules into Application services). This is the natural next step.
- Implement an `IPlatformFileService` and replace `FilePicker.PickAsync` calls in `MainPage` (Task 005/007).
- Align `ResultsLoader` exception behavior with `IResultsLoader` docs (either rethrow `JsonException` or update interface/docs and UI catch blocks).
- Add Android storage permission handling and document scoped-storage fallbacks (Task 004/008).
- Optional: add unit tests for `ResultsLoader`, `DuplicateNavigator`, and `ImageRemovalService` to assert business rules live in the Application layer.

Files referenced
- `src/ImageDuplicateSearcher.Application/Services/ResultsLoader.cs`
- `src/ImageDuplicateSearcher.Application/Services/DuplicateNavigator.cs`
- `src/ImageDuplicateSearcher.Application/Services/ImageDisplayManager.cs`
- `src/ImageDuplicateSearcher.Application/Services/ImageRemovalService.cs`
- `src/ImageDuplicateSearcher.Desktop/MainPage.xaml.cs`
- `src/ImageDuplicateSearcher.Desktop/ViewModels/ImageTileViewModel.cs`
- `src/ImageDuplicateSearcher.Desktop/MauiProgram.cs`
- `src/ImageDuplicateSearcher.Desktop/Platforms/Android/AndroidManifest.xml`

Conclusion
- High-level architectural boundary (business rules in Application project, presentation in MAUI) is implemented correctly. The remaining work is primarily platform adapters and small alignment fixes described above.
