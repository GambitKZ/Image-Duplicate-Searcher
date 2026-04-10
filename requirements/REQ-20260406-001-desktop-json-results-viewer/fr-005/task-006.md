# Task 006 — Create Android File Adapter

Summary
- Implement the Android variant of `IPlatformFileService` that requests runtime storage permissions (or uses SAF/scoped-storage flow) and returns picked JSON file path(s).

Goal
- Platform-safe file picking on Android with graceful fallback for scoped-storage constraints.

Deliverable
- `AndroidPlatformFileService` implementation and DI registration; small sample code showing permission request flow.

Estimate
- 1.5 hours

Dependencies
- Task 004, Task 005
