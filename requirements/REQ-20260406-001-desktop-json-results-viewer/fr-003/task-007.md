# Task 007 — Bind and populate image tiles in `MainPage.xaml.cs`

Goal

- Populate a bound collection of image tiles when the navigator's current group changes and asynchronously load image bytes for each tile.

Deliverables

- Updated `src/ImageDuplicationSearcher.Desktop/MainPage.xaml.cs`
- Optional small desktop-side view model (`src/ImageDuplicationSearcher.Desktop/ViewModels/ImageTileViewModel.cs`) if needed for binding.

Description

- Inject `IImageDisplayManager` into `MainPage` constructor.
- On navigator `CurrentGroup` change, map `DuplicateSearchResultImage` entries to `ImageTileModel` and create a bound `ObservableCollection` for the UI.
- For each tile, call `GetImageDisplayAsync(imagePath)` to receive bytes/placeholder and set an `ImageSource` on the UI element via `ImageSource.FromStream`.
- Keep UI responsive by running image loads on background threads and updating the UI on the main thread.
- Do not modify disk files or implement removal—only render and show placeholder reasons.

Acceptance Criteria

- When a group is active, all non-deleted images are displayed with size metadata.
- Missing or unreadable images show the placeholder plus a short reason visible in the tile UI.
- Scrolling and navigation remain responsive.

Estimated time

- 2 hours

Dependencies

- Tasks 001–006
