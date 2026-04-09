# Task 006 — Implement Jump-to-Group validation and boundary UX

Goal
- Provide input validation and correct boundary behavior for navigation controls.

Deliverable
- Update `MainPage.xaml`/`MainPage.xaml.cs` to:
  - Validate `GroupEntry` input is integer and within `1..TotalCount` before calling `TryGoToGroup`.
  - Disable `PrevButton` when `CanMovePrevious` is false and `NextButton` when `CanMoveNext` is false.
  - Show validation messages for invalid inputs (reuse `ErrorLabel` or add `ValidationLabel`).

Acceptance criteria
- Entering `0`, `-1`, `N+1`, or non-numeric input shows a user-friendly message and does not call the navigator. `Prev/Next` are disabled at boundaries.

Estimate
- 45–90 minutes

Notes
- Keep validation synchronous and UI-focused; no server calls.
