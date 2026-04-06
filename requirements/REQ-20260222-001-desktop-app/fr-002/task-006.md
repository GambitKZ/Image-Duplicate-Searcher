## Task 006 — Update WorkflowService to read runtime options

Goal
- Ensure the scanning `WorkflowService` reads the runtime `ImageDuplicationOptions` instance instead of only reading `appsettings.json` on startup.

Deliverable
- Modify `WorkflowService` (or its callers) to accept the DI options instance and use its values for scans.
- Add a small integration check to ensure scans use the UI-provided `Supported Formats` and paths.

Estimate
- 60–120 minutes

Dependencies
- Tasks 003, 004, 005
