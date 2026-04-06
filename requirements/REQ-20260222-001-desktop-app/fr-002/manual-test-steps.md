# Manual Test Steps — FR-002 UI: Inputs for Paths and Formats

These steps verify the UI controls for `ImageDirectory`, `OutputFilePath`, and `Supported Formats` described in FR-002.

Prerequisites
- Build the solution and launch the Desktop app (see README or run from the IDE).
- Have a sample images folder with mixed file extensions (`.jpg`, `.png`, `.bmp`, etc.).

Test 1 — Set Image Directory
- Open the Desktop app main window.
- Click the `Set Image Directory` button.
- In the folder/file picker select (or pick a file inside) the sample images folder.
- Expected: The `ImageDirectory` entry shows the selected folder path.

Test 2 — Set Output File
- Click the `Set Output File` button.
- Choose (or type) a path where the output should be written (e.g., `C:\temp\duplicates.json`).
- Expected: The `Output File` entry shows the selected path.

Test 3 — Edit Supported Formats and run scan
- In the `Supported Formats` Entry, enter `jpg;png` (without quotes) and press Enter or leave the field.
- Click `Start Scan`.
- Observe the UI `Response` messages for processing lines.
- Expected: The scan only processes files with `.jpg` and `.png` extensions; `.bmp` or other extensions are ignored.

Test 4 — Ensure formats are normalized
- Enter formats with and without leading dots and mixed casing, e.g. `JPG;.Png; gif`.
- Start a scan (point `ImageDirectory` to a folder containing those types).
- Expected: The runtime uses normalized formats `['.jpg', '.png', '.gif']` (case-insensitive) when filtering files.

Test 5 — In-memory only (no appsettings.json changes)
- Change entries in the UI and run scans.
- Restart the application.
- Expected: UI changes are not persisted to `appsettings.json`; after restart fields revert to configured defaults.

Notes
- On platforms where a folder picker is not available, the code falls back to `Entry` fields — verify by manually typing a valid path.
- If no images are found or the folder is invalid, the UI will report an error (mirrors console behavior).

Acceptance Criteria Mapping
- Selecting folder/file updates corresponding fields — Tests 1 & 2
- Editing Supported Formats used for subsequent scans (in-memory) — Tests 3 & 4
- Formats accept `;` as separator and normalize extensions — Tests 3 & 4
