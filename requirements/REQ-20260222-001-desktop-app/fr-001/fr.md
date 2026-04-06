# REQ-20260222-001:FR-001 — MAUI Desktop Host

# User Story
As a user, I want the application to run as a desktop app so that I can use a GUI to run the duplicate-image workflow.

# Requirement
The system MUST provide a MAUI-based Desktop host project that can run the existing `WorkflowService`, `ImageProcessor`, and `Reporter` services and execute the duplicate-image workflow from a GUI.

# Acceptance Criteria (Given / When / Then)
- Given a built MAUI desktop app with services registered
- When the user launches the application
- Then the app MUST start and present the main UI and MUST be able to trigger the same workflow that the Console app runs.

# Test Outline
1. Build and launch the MAUI desktop project on the target platform.
2. Verify the main window appears and services are registered (smoke test).
3. Trigger the workflow from the UI and verify the workflow completes and returns results.

# Rationale
Providing a desktop host lets users run the workflow in a graphical environment while reusing existing business logic and services.
