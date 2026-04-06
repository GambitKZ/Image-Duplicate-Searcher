# REQ-20260222-001:FR-005 — UI-aware Reporter

# User Story
As a user, I want to see live status and final results in the Desktop UI so I can monitor the scan and inspect duplicates without using the console.

# Requirement
The system MUST provide a UI-aware `IReporter` implementation (or adapter) that exposes real-time log/status messages to the UI (via events, callbacks, or `IObservable<string>`) and MUST still save results to the configured `OutputFilePath` as JSON.

# Acceptance Criteria (Given / When / Then)
- Given a running scan
- When the `ImageProcessor` or `WorkflowService` emits status messages
- Then the UI-aware `IReporter` MUST forward those messages to the UI Response area in near real-time and write the final duplicate groups to the selected `OutputFilePath`.

# Test Outline
1. Start a scan and verify UI log lines appear as processing proceeds.
2. After completion, verify the JSON file is created at `OutputFilePath` and contains the expected duplicate groups.

# Rationale
Separating UI reporting from console reporting preserves the single-responsibility of reporters and provides a clean integration point for the GUI.
