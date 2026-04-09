# Task 006 — Pass source model and removal service from `MainPage` to tiles

Goal
- Ensure each created tile has access to the underlying `DuplicateSearchResultImage` instance and the `IImageRemovalService` so it can mark `IsDeleted` and trigger UI refresh.

Deliverable
- Update `src/ImageDuplicationSearcher.Desktop/MainPage.xaml.cs`:
  - Accept `IImageRemovalService` via constructor injection and store as `_imageRemovalService`.
  - When building tiles in `RefreshTilesAsync`, construct tiles using the new constructor: `new ImageTileViewModel(sourceModel, _imageRemovalService, onRemoved: () => RefreshTilesAsync())`.
  - Ensure `RefreshTilesAsync` is safe for re-entry and invoked on the main thread as required.

Acceptance Criteria
- Tiles are constructed with the source model and service; invoking the tile's `RemoveCommand` results in `RefreshTilesAsync` being called and the deleted image no longer shown.

Estimated Effort
- 45–60 minutes

Dependencies
- Task 005, Task 004

Notes
- Keep constructor changes in `MainPage` minimal; update dependency registrations in `MauiProgram.cs` as needed.