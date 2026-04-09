# Task 002 — Add image tile view model (`ImageTileModel`)

Goal

- Add a view-model shape that the UI can bind to for each image tile in a duplicate group.

Deliverables

- `src/ImageDuplicateSearcher.Application/Models/ImageTileModel.cs`

Description

- Create `ImageTileModel` with properties:
  - `string Path`
  - `double SizeMB`
  - `bool IsDeleted` (reuse existing semantics)
  - `ImageRenderStatus RenderStatus` (enum: `Available`, `Missing`, `Unreadable`)
  - `string? PlaceholderReason`

Acceptance Criteria

- Model compiles and maps cleanly from `DuplicateSearchResultImage`.
- No UI framework dependencies.

Estimated time

- 30 minutes

Dependencies

- Task 001 (optional) for RenderStatus/placeholder semantics
