# REQ-20260406-001:FR-003 — Render Group Images

# Design Considerations
- Rendering logic SHOULD call Application-layer abstraction (`IImageDisplayManager`) so file-access and fallback rules stay outside UI code.
- Placeholder handling MUST be deterministic and platform-consistent.
- Image metadata (sizeMB from JSON) MUST be shown even when file rendering fails.

# Data Flow
1. UI asks navigator for active group images.
2. For each image entry, UI requests display source from `IImageDisplayManager`.
3. Service returns real image source or placeholder plus reason; UI renders tile with path and size.

# Affected Components (Projects, Services, Classes)
- ImageDuplicateSearcher.Application: `IImageDisplayManager` and result tile model
- ImageDuplicationSearcher.Desktop: collection/grid canvas and item template

# Dependencies
- MAUI image controls for rendering
- Optional ImageSharp validation in Application display manager

# Implementation Steps
1. Define view model shape for image tile including render status and optional reason.
2. Implement display manager fallback behavior for missing/unreadable files.
3. Build responsive, scrollable image grid in Desktop UI and bind tile metadata.