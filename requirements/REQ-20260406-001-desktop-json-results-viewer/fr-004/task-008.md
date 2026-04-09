# Task 008 — Add user-facing error/success feedback flow and refresh logic

Goal
- Provide clear, actionable messages to users when removal succeeds or fails and ensure source JSON is never modified.

Deliverable
- Update `MainPage.xaml.cs` to display success toasts or status labels when removal succeeds.
- On failure, show an actionable error (e.g., "Permission denied" with suggestion to close other apps, or "File missing"), and do not change in-memory `IsDeleted` state.
- Ensure there are no code paths that write back to the source JSON file.

Acceptance Criteria
- Successful removal shows success messaging and tile disappears after refresh.
- Failure leaves in-memory state unchanged and shows actionable messaging.
- Verified that source JSON file is not written to by the removal flow.

Estimated Effort
- 45–60 minutes

Dependencies
- Task 003, Task 005, Task 006

Notes
- Integrate with existing `StatusLabel`/`ErrorLabel` or `DisplayAlertAsync` helper patterns already present in `MainPage`.