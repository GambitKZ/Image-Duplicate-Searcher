# REQ-20260406-001:FR-005 — Platform And Layer Boundaries

# Design Considerations
- Application-layer services MUST encapsulate load, navigation, rendering state, and removal rules.
- MAUI layer SHOULD map platform capabilities (file picker, permissions) without embedding domain behavior.
- Android support SHOULD document any scoped-storage constraints and user-facing fallbacks.

# Data Flow
1. UI command invokes Application service abstraction.
2. Service executes business rule and returns state/result.
3. UI renders returned state and handles platform-specific interaction affordances.

# Affected Components (Projects, Services, Classes)
- ImageDuplicateSearcher.Application: domain/service implementations for Desktop browsing workflow
- ImageDuplicationSearcher.Desktop: platform-specific adapters and visual layer

# Dependencies
- MAUI platform services for file access and permissions
- Application service interfaces from FR-001 through FR-004

# Implementation Steps
1. Register Application services in `MauiProgram` and bind to view-models.
2. Keep view-model logic thin and delegate rules to Application services.
3. Add platform notes and fallback behavior for Android storage constraints.