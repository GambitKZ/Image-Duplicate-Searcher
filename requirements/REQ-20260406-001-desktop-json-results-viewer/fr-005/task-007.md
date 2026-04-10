# Task 007 — Refactor MainPage File Picking

Summary
- Replace direct `FilePicker.PickAsync` usage in `MainPage` with the new `IPlatformFileService.PickJsonFileAsync()` and keep UI orchestration only.

Goal
- Move platform interactions behind the adapter, keeping `MainPage` focused on presentation and user interaction only.

Deliverable
- PR updating `MainPage` to use `IPlatformFileService` and small manual verification steps.

Estimate
- 1.0 hour

Dependencies
- Task 005, Task 006
