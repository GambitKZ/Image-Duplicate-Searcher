# REQ-20260406-001:FR-004 — Remove Images In Session

# User Story
As a user, I want to remove an image from the current review flow so that I can clean up duplicates while preserving result provenance.

# Requirement
The system MUST provide a Remove action per image that MUST attempt to delete the underlying file and MUST update in-memory state (for example `IsDeleted`) without modifying the source JSON file.

# Acceptance Criteria (Given / When / Then)
- Given an image tile in the active group
- When the user clicks Remove and deletion succeeds
- Then the app MUST mark the image as deleted in memory, MUST remove it from active visual results, and SHOULD show a success status.
- Given deletion fails because of permissions, lock, or missing file
- When the remove action is executed
- Then the app MUST keep source JSON unchanged, SHOULD retain consistent in-memory state, and MUST show an actionable error message.
- Given the session continues after removals
- When the user navigates groups
- Then the app MUST use the in-memory state rather than re-reading and rewriting the source JSON.

# Test Outline
1. Remove an existing image and verify file deletion and in-memory status update.
2. Attempt to remove a locked or unauthorized image and verify graceful failure messaging.
3. Confirm the originally selected JSON file remains byte-for-byte unchanged.

# Rationale
Session-based state allows interactive cleanup workflows while preserving the original result file as an auditable snapshot.