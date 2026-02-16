# REQ-20260216-001:FR-006 — Report Duplicate Image Groups

# User Story
As a user, I want to see the results of duplicate detection so that I can review and act on them.

# Acceptance Criteria (Given / When / Then)
- Given image groups have been identified
- When the report is generated
- Then duplicate groups MUST be displayed in the console and saved to a JSON file

# Test Outline
1. Process images to identify duplicates.
2. Generate the report.
3. Verify console displays the groups and a JSON file is created with the data.

# Rationale
Reporting provides the user with clear, actionable information about duplicate images found.