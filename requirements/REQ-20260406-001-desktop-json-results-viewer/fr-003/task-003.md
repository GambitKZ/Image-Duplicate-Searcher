# Task 003 — Implement `ImageDisplayManager` service (Application)

Goal

- Implement `ImageDisplayManager` in `ImageDuplicateSearcher.Application.Services` to load images, validate them with ImageSharp, and return `ImageDisplayResult`.

Deliverables

- `src/ImageDuplicateSearcher.Application/Services/ImageDisplayManager.cs`

Description

- Use `SixLabors.ImageSharp` to open and validate image files.
- If file is missing/unreadable, return placeholder bytes with `IsPlaceholder=true` and a short `Reason` (e.g., "Missing file", "Unreadable image").
- If file is valid, return the raw image bytes and `IsPlaceholder=false`.
- Add lightweight in-memory cache to avoid repeated disk reads on navigation.
- Ensure file I/O runs off the UI thread and methods are `async`.

Acceptance Criteria

- Service compiles and passes basic smoke tests (loads a valid file, returns placeholder for a missing file).
- No MAUI dependencies introduced in Application project.

Estimated time

- 2 hours

Dependencies

- Task 001 (interface/DTO)
- Task 004 (placeholder resource)
