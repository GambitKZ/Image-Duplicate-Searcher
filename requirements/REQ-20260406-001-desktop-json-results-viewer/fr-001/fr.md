# REQ-20260406-001:FR-001 — Load JSON Search Results

# User Story
As a user, I want to select a JSON results file so that I can browse duplicate groups in the Desktop app.

# Requirement
The system MUST allow the user to select a JSON search-result file and MUST deserialize it into an in-memory model compatible with the Reporter output structure.

# Acceptance Criteria (Given / When / Then)
- Given the user is on the Desktop app main page
- When the user clicks "Open JSON Results" and selects a valid JSON file
- Then the app MUST load the file into memory and MUST show that data is ready for browsing.
- Given the user selects an invalid or malformed JSON file
- When deserialization fails
- Then the app MUST show a clear error message and SHOULD keep the previous in-memory state unchanged.

# Test Outline
1. Open the app and select a valid JSON output file from the Console workflow.
2. Verify the app loads data and enables browsing controls.
3. Select an invalid JSON file and verify an error is shown without corrupting in-memory state.

# Rationale
Loading search results from JSON decouples Desktop browsing from search execution and enables review of previously generated results.