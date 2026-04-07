# REQ-20260406-001:FR-002 — Navigate Duplicate Groups

# User Story
As a user, I want to navigate duplicate groups by simple group number so that I can quickly inspect results.

# Requirement
The system MUST provide group-based navigation using 1..N indexing and MUST support Previous, Next, and Jump-to-Group behaviors while still showing each group's hash value.

# Acceptance Criteria (Given / When / Then)
- Given results are loaded with N duplicate groups
- When the user views the summary area
- Then the app MUST show total group count and current position as "X / N".
- Given the user is on any group
- When the user clicks Previous or Next
- Then the app MUST move by one group within valid boundaries and MUST disable navigation buttons when boundaries are reached.
- Given the user enters a valid group number
- When the user clicks Go
- Then the app MUST jump to that group and SHOULD keep the displayed hash visible.

# Test Outline
1. Load a JSON file with multiple groups and verify total count appears.
2. Use Previous/Next to move between groups and verify boundary behavior.
3. Use Jump-to-Group with valid and invalid inputs and verify expected navigation and validation messaging.

# Rationale
Numbered navigation reduces cognitive load compared to hash-only navigation while preserving hash visibility for traceability.