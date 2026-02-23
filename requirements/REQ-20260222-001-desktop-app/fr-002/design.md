# REQ-20260222-001:FR-002 — UI: Inputs for Paths and Formats

# Design Considerations
- Use MAUI `FolderPicker` or platform-specific file dialogs where available; fall back to an `Entry` textbox for manual input on platforms where folder picking is limited.
- The `Supported Formats` field SHOULD accept a list with `;` separation and be parsed into a string[] for `ImageDuplicationOptions`.
- The UI MUST not persist changes to `appsettings.json` by default (per configuration decision); it MUST apply changes in-memory for the running session. Though the default one can be picked from `appsettings.json`.

# Data Flow
1. User clicks a picker button; platform dialog returns a path.
2. The UI updates the corresponding `Entry` control and updates the in-memory `ImageDuplicationOptions` instance.

# Affected Components
- `MainPage.xaml` / `MainPage.xaml.cs` UI
- `ImageDuplicationOptions` model
- `WorkflowService` invocation points

# Implementation Steps
1. Add UI entries and buttons to `MainPage.xaml` for `ImageDirectory`, `OutputFilePath`, and `Supported Formats`.
2. Implement click handlers in `MainPage.xaml.cs` to open pickers and update `ImageDuplicationOptions` in DI or a local view-model.
3. Ensure UI validation for supported formats (trim, ensure leading dot for extensions).
