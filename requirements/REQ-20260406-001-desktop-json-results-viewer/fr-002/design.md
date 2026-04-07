# REQ-20260406-001:FR-002 — Navigate Duplicate Groups

# Design Considerations
- Navigation state MUST be centralized in an Application-layer service (`IDuplicateNavigator`) to keep UI logic thin.
- Group labels SHOULD be 1-based for user display, while internal storage MAY use 0-based indexing.
- Hash values MUST remain visible in the detail view for the active group.

# Data Flow
1. `IDuplicateNavigator.Initialize(results)` receives loaded groups.
2. UI requests current index and total count to render summary.
3. UI invokes `Previous`, `Next`, or `GoToGroup` and refreshes active group view.

# Affected Components (Projects, Services, Classes)
- ImageDuplicateSearcher.Application: `IDuplicateNavigator` and implementation
- ImageDuplicationSearcher.Desktop: navigation controls and binding/view-model updates

# Dependencies
- In-memory duplicate result model from FR-001

# Implementation Steps
1. Implement navigation service with boundary validation.
2. Expose current position, current group, and total count for UI binding.
3. Add Desktop controls for Previous/Next/Jump and map commands to service methods.