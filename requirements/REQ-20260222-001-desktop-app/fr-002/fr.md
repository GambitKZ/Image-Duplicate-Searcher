# REQ-20260222-001:FR-002 — UI: Inputs for Paths and Formats

# User Story
As a user, I want to set the `ImageDirectory`, `OutputFilePath`, and supported formats through the UI so that I can control the scan inputs without editing configuration files.

# Requirement
The Desktop UI MUST provide controls to set `ImageDirectory` and `OutputFilePath` (via buttons and text fields) and a `Supported Formats` editable field that mirrors the `SupportedFormats` array from `appsettings.json`.

# Acceptance Criteria (Given / When / Then)
- Given the main window is open
- When the user clicks the "Set Image Directory" button and selects a folder
- Then the `ImageDirectory` field MUST display the selected path.

- Given the main window is open
- When the user clicks the "Set Output File" button and selects a file path
- Then the `OutputFilePath` field MUST display the selected path.

- Given the main window is open
- When the user edits the `Supported Formats` field
- Then the edited value MUST be used for subsequent scans (in-memory), and MUST accept extensions separated by `;` sign.

# Test Outline
1. Use the UI to select a sample image directory and verify the path appears in the field.
2. Use the UI to select an output file and verify the path appears in the field.
3. Edit the Supported Formats field and run a scan; verify the `ImageProcessor` only considers files with the specified extensions.

# Rationale
Providing these inputs in the UI removes the need to edit `appsettings.json` and makes the tool easier to use for non-developers.
