# REQ-20260406-001:FR-005 — Platform And Layer Boundaries

# User Story
As a product owner, I want clear platform and architectural boundaries so that the Desktop feature is maintainable and consistent.

# Requirement
The system MUST support Android and Windows for the JSON-results browsing workflow, and business logic MUST be implemented in the Application project while the MAUI layer focuses on presentation and user interaction.

# Acceptance Criteria (Given / When / Then)
- Given the Desktop app is built for Windows
- When a user performs load, navigate, render, and remove actions
- Then the app MUST complete the workflow with platform-appropriate file and permission handling.
- Given the Desktop app is built for Android
- When a user performs the same actions
- Then the app SHOULD complete the same workflow with documented platform-specific limitations where applicable.
- Given code review of implementation
- When architecture boundaries are evaluated
- Then search/browse business rules MUST reside in Application services and MAUI code SHOULD remain orchestration/presentation only.

# Test Outline
1. Execute core workflow on Windows and verify end-to-end behavior.
2. Execute core workflow on Android and verify parity for supported operations.
3. Verify business logic classes are in Application project and UI layer does not duplicate rules.

# Rationale
Clear boundaries reduce regression risk, improve testability, and keep cross-platform behavior predictable.