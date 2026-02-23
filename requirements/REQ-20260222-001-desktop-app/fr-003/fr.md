# REQ-20260222-001:FR-003 — UI: Start Scan and Background Execution

# User Story
As a user, I want to start the duplicate-image scan from the UI and see progress without the UI freezing.

# Requirement
The Desktop UI MUST include a `Start Scan` button that runs the workflow on a background thread/task and MUST provide progress or status updates to the UI while running.

# Acceptance Criteria (Given / When / Then)
- Given valid input values are present in the UI
- When the user clicks `Start Scan`
- Then the workflow MUST run asynchronously, the UI MUST remain responsive, and status updates MUST appear in the Response area.

# Test Outline
1. Start a scan on a directory with many images and verify the UI remains responsive.
2. Verify progress/status lines appear in the Response area during execution.

# Rationale
Running long tasks on the UI thread would block interaction. Background execution with progress updates improves usability.
