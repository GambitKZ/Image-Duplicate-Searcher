# Task 001 — Add IImageRemovalService interface

Goal
- Define a clear contract for image removal operations in the Application layer so Desktop/UI can call a single service to attempt file deletion.

Deliverable
- Create `IImageRemovalService.cs` under `src/ImageDuplicateSearcher.Application/Interfaces` with a single method:

  - `Task<RemovalResult> RemoveImageAsync(string path);`

Acceptance Criteria
- Interface compiles and is visible to Desktop project via existing project references.

Estimated Effort
- 30 minutes

Dependencies
- Task 002: `RemovalResult` enum

Notes
- Keep the interface minimal; error mapping is done via `RemovalResult` rather than throwing raw exceptions to simplify UI handling.