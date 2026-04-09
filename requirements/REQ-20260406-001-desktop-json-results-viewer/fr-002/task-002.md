# Task 002 — Implement `DuplicateNavigator` service

Goal
- Provide an Application-layer implementation of `IDuplicateNavigator` that manages group state, enforces boundaries, and notifies UI consumers.

Deliverable
- Add `src/ImageDuplicateSearcher.Application/Services/DuplicateNavigator.cs` implementing the interface. Responsibilities:
  - Store the provided `DuplicateSearchResult[]` and maintain an internal 0-based index.
  - Implement `Previous`, `Next`, and `TryGoToGroup` with boundary validation.
  - Expose `TotalCount`, `CurrentIndex`, `CurrentGroup`, `CanMovePrevious`, and `CanMoveNext`.
  - Raise property change notifications (implement `INotifyPropertyChanged` or a `CurrentGroupChanged` event) so the UI can update.

Acceptance criteria
- Service compiles and passes basic manual verification: initialize with test data, move previous/next, and verify properties update and events fire.

Estimate
- 90–120 minutes

Notes
- Keep the implementation thread-safe for simple UI usage; avoid heavy synchronization.
