# Task 002 — Add `RemovalResult` enum for outcome mapping

Goal
- Provide a small enum that represents the canonical outcome of a removal attempt so the service and UI can agree on user-facing messaging.

Deliverable
- Create `RemovalResult.cs` under `src/ImageDuplicateSearcher.Application/Models` containing an enum such as:
  - `Success`
  - `NotFound`
  - `Unauthorized`
  - `Locked`
  - `UnknownError`

Acceptance Criteria
- Enum compiles and can be referenced by `IImageRemovalService` and the concrete service implementation.

Estimated Effort
- 15–30 minutes

Dependencies
- None (used by Task 001 and Task 003)

Notes
- Keep values stable; map common filesystem exceptions to the enum in the implementation.