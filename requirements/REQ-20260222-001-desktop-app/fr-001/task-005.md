# Task 005 — Add async entry point and cancellation to WorkflowService

Goal
Ensure `WorkflowService` exposes an async `ExecuteWorkflowAsync(ImageDuplicationOptions, IProgress<string>, CancellationToken)` method that supports progress reporting and cancellation.

Deliverable
- Add or modify `WorkflowService` to include the async method and hook progress callbacks into `Reporter`.
- Update callers as needed.

Estimated time
1–2 hours

Dependencies
- Task 001
- Task 004
