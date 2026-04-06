# Task 002 — Add appsettings.json loading to MAUI host

Goal
Add configuration loading for `appsettings.json` in the Desktop project so the MAUI host can read `ImageDuplicationOptions` defaults.

Deliverable
- Update `MauiProgram.cs` to load `appsettings.json` (when present) into IConfiguration and bind `ImageDuplicationOptions` as options.
- Provide fallback defaults if the file is missing.

Estimated time
30–60 minutes

Dependencies
- Task 001
