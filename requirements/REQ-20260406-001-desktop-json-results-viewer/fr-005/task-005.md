# Task 005 — Create Windows File Adapter

Summary
- Add a platform abstraction `IPlatformFileService` to the Desktop project and implement the Windows variant that exposes `Task<string?> PickJsonFileAsync()` and permission-check helpers.

Goal
- Provide a platform-neutral API for the UI to open JSON files without embedding FilePicker logic directly in UI code.

Deliverable
- New `IPlatformFileService` interface, `WindowsPlatformFileService` implementation, and DI registration in `MauiProgram.cs`.

Estimate
- 1.0 hour

Dependencies
- Task 003
