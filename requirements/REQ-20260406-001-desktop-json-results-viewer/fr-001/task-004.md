# Task 004 — Wire Desktop load flow to Application service

Goal
Connect MAUI file selection to the Application `IResultsLoader` service without implementing navigation logic from FR-002.

Deliverable
- Register `IResultsLoader` in Desktop dependency injection.
- Add UI command/handler to invoke file picker and call loader.
- Keep scope limited to load-success/load-failure state.

Estimated time
45–90 minutes

Dependencies
- Task 002
- Task 003