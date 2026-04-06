## Task 005 — Wire UI fields to the in-memory options

Goal
- Connect the `Entry` controls and picker handlers to the DI-registered in-memory options instance so changes apply immediately for the running session.

Deliverable
- `MainPage.xaml.cs` or ViewModel updates that write user selections to the shared `ImageDuplicationOptions` instance.
- Basic UI validation feedback (e.g., invalid formats highlighted, required fields non-empty).

Estimate
- 45–90 minutes

Dependencies
- Tasks 001, 002, 004
