# Task 003 — Implement `ImageRemovalService` with safe delete logic

Goal
- Implement a robust Application-layer service that attempts to delete a file from disk and maps exceptions to `RemovalResult` values.

Deliverable
- Add `ImageRemovalService.cs` under `src/ImageDuplicateSearcher.Application/Services` implementing `IImageRemovalService`.
- Implementation details:
  - Use `File.Exists` and `File.Delete` with `try/catch`.
  - Catch and map `UnauthorizedAccessException`, `IOException`, `FileNotFoundException`, and other exceptions to the enum values.
  - Do not mutate in-memory domain models here; return outcome to caller.

Acceptance Criteria
- Service compiles and returns correct `RemovalResult` values for simulated exception cases.

Estimated Effort
- 60–90 minutes

Dependencies
- Task 001: `IImageRemovalService`
- Task 002: `RemovalResult`

Notes
- Keep the service side-effect to file system only (no JSON writes). The UI or caller will mark in-memory state on success.