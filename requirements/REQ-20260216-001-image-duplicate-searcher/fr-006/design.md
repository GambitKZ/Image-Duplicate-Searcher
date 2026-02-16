# REQ-20260216-001:FR-006 — Report Duplicate Image Groups

# Design Considerations
- Use Spectre.Console for console output
- Filter groups with more than one image
- Save to JSON format
- Include file paths and group info

# Data Flow
1. Receive grouped dictionary
2. Filter for duplicates (groups >1)
3. Display in console with formatting
4. Serialize to JSON and save to file

# Affected Components (Projects, Services, Classes)
- Reporter

# Dependencies
- Spectre.Console for UI
- System.Text.Json for serialization

# Implementation Steps
1. Process the groups dictionary
2. Output to console
3. Write JSON to configured file path