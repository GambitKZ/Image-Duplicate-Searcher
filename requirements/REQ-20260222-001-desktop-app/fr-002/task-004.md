## Task 004 — Expose in-memory ImageDuplicationOptions via DI

Goal
- Provide a runtime, in-memory instance of `ImageDuplicationOptions` that the UI can update and other services can consume.

Deliverable
- Register a scoped/singleton service (e.g., `IRuntimeOptions` or `ImageDuplicationOptions`) in `ConfigureServices.cs`.
- Ensure the DI registration uses a mutable instance that the UI updates at runtime (no persistence to `appsettings.json`).

Estimate
- 30–60 minutes

Dependencies
- Task 003
