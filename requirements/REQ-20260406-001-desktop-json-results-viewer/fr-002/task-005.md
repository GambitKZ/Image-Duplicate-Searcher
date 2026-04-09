# Task 005 — Wire `MainPage` to `IDuplicateNavigator` and initialize on load

Goal
- Connect the Desktop UI to the navigation service and use it to render and control the active duplicate group.

Deliverable
- Modify `src/ImageDuplicateSearcher.Desktop/MainPage.xaml.cs`:
  - Accept `IDuplicateNavigator` in the constructor and store a reference.
  - After results are loaded (`OnOpenJsonResults`), call `_navigator.Initialize(results)` instead of only caching local results.
  - Subscribe to property-change/event notifications from the navigator to update `SummaryLabel`, `GroupHashLabel`, and enable/disable navigation buttons.
  - Implement `OnPrevClicked`, `OnNextClicked`, and `OnGoClicked` handlers that call the navigator methods.

Acceptance criteria
- Loading a JSON results file sets `SummaryLabel` to `"1 / N"`, controls call navigator methods, and UI updates when the active group changes.

Estimate
- 60–120 minutes

Notes
- Keep UI update code simple; prefer small helper methods to refresh the visible state.
