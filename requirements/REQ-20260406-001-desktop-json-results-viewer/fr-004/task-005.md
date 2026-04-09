# Task 005 — Update `ImageTileViewModel` to accept source model and add `RemoveCommand`

Goal
- Allow each tile to initiate removal and update the in-memory model when deletion succeeds.

Deliverable
- Modify `src/ImageDuplicationSearcher.Desktop/ViewModels/ImageTileViewModel.cs`:
  - Add a constructor overload that accepts `DuplicateSearchResultImage` (source model), `IImageRemovalService`, and an `Action` or `Func<Task>` callback named `onRemoved`.
  - Add an `ICommand` property `RemoveCommand` that runs the removal flow:
    - Call `RemoveImageAsync(path)` on the injected `IImageRemovalService`.
    - On `RemovalResult.Success`, set the source model's `IsDeleted = true` (or call `onRemoved`), update view-model properties (e.g., `IsPlaceholder`), and invoke provided `onRemoved` callback to trigger UI refresh.
    - On failure, surface an actionable error via `onRemoved` callback or via UI messaging mechanism.

Acceptance Criteria
- `RemoveCommand` is present and can be executed in code; it calls the service and triggers the callback on success.

Estimated Effort
- 60–90 minutes

Dependencies
- Task 001, Task 003, Task 004

Notes
- Keep UI-thread transitions (MainThread.BeginInvokeOnMainThread) for property changes and callback invocation.