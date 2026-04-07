# REQ-20260406-001:FR-004 — Remove Images In Session

# Design Considerations
- Removal rules MUST be implemented in Application service (`IImageRemovalService`) with explicit error handling.
- In-memory model SHOULD carry deletion state (`IsDeleted`, `DeletedAt`) independent of source JSON.
- The source JSON file MUST NOT be edited or overwritten by removal operations.

# Data Flow
1. User presses Remove on an image tile.
2. Desktop UI calls `IImageRemovalService.RemoveImageAsync(path)`.
3. On success, UI marks item as deleted in-memory and refreshes current group projection.
4. On failure, UI displays error and preserves coherent in-memory/session state.

# Affected Components (Projects, Services, Classes)
- ImageDuplicateSearcher.Application: `IImageRemovalService`, in-memory state updater
- ImageDuplicationSearcher.Desktop: remove command, toast/status messaging, refresh behavior

# Dependencies
- File system delete operations
- In-memory session state model

# Implementation Steps
1. Extend in-memory image result model with deletion state.
2. Implement safe delete logic and map exceptions to user-facing status outcomes.
3. Ensure UI refreshes from in-memory state and never writes back to source JSON.