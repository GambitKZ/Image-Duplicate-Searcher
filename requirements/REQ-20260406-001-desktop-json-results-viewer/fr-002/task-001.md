# Task 001 — Add `IDuplicateNavigator` interface

Goal
- Define a navigation contract in the Application layer to centralize navigation state and behavior.

Deliverable
- Add `src/ImageDuplicateSearcher.Application/Interfaces/IDuplicateNavigator.cs` containing a documented interface with the following surface:
  - `void Initialize(DuplicateSearchResult[] groups)`
  - `int TotalCount { get; }`
  - `int CurrentIndex { get; }` (0-based internally)
  - `DuplicateSearchResult? CurrentGroup { get; }`
  - `bool CanMovePrevious { get; }`
  - `bool CanMoveNext { get; }`
  - `void Previous()` and `void Next()`
  - `bool TryGoToGroup(int displayIndex)` (accepts 1-based index, returns success)
  - Either implement `INotifyPropertyChanged` or expose an event so UI can react to changes.

Acceptance criteria
- Interface compiles and documents the contract. UI code can reference `IDuplicateNavigator` for binding.

Estimate
- 30–60 minutes

Notes
- Keep UI-facing indexing 1-based; document mapping to internal 0-based index in the interface XML comments.
