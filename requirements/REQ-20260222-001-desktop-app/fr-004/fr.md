# REQ-20260222-001:FR-004 — DI and Configuration Integration

# User Story
As a developer, I want the Desktop app to reuse existing services and configuration so that business logic is not duplicated.

# Requirement
The MAUI Desktop host MUST register and resolve the same `WorkflowService`, `ImageProcessor`, and `Reporter` implementations as the Console application using the existing service registration extension; the UI MUST be able to override `ImageDuplicationOptions` values at runtime without changing persisted configuration by default.

# Acceptance Criteria (Given / When / Then)
- Given the MAUI app starts
- When the DI container is built
- Then the container MUST be able to resolve `WorkflowService`, `IImageProcessor`, and `IReporter` and the UI MUST be able to supply an `ImageDuplicationOptions` instance used by the workflow.

# Test Outline
1. Launch the MAUI app and confirm DI resolution for `WorkflowService`.
2. In the UI, set overrides for options and start a scan; confirm the overrides are used during the run.

# Rationale
Reusing DI and services reduces code duplication and keeps behavior consistent across console and desktop hosts.
