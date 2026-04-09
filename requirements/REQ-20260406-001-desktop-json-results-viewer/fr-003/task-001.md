# Task 001 — Define `IImageDisplayManager` contract and `ImageDisplayResult` DTO

Goal

- Add an Application-layer contract that centralizes file access, validation, and placeholder behavior for image rendering.

Deliverables

- `src/ImageDuplicateSearcher.Application/Interfaces/IImageDisplayManager.cs`
- `src/ImageDuplicateSearcher.Application/Models/ImageDisplayResult.cs`

Description

- Define `ImageDisplayResult` as a small DTO with fields: `byte[] ImageBytes`, `bool IsPlaceholder`, and `string? Reason`.
- Define `IImageDisplayManager` with methods:
  - `Task<ImageDisplayResult> GetImageDisplayAsync(string imagePath)`
  - `Task<byte[]> GetImageBytesAsync(string imagePath)` (optional convenience)
  - `bool IsImageAccessible(string imagePath)`

Acceptance Criteria

- Interface and DTO compile in the Application project without adding MAUI references.
- Methods are documented with XML comments.

Estimated time

- 45 minutes

Dependencies

- None (can be implemented and reviewed independently)
