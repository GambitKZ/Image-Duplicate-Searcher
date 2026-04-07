# REQ-20260406-001:FR-003 — Render Group Images

# User Story
As a user, I want to see all images in the selected duplicate group so that I can compare them visually.

# Requirement
The system MUST render all images of the active group in a canvas-like visual area, MUST show each image size, and MUST render a placeholder with a reason when an image cannot be displayed.

# Acceptance Criteria (Given / When / Then)
- Given a group is selected
- When the image panel is rendered
- Then the app MUST display all non-deleted group images with their file size values.
- Given an image file is missing, inaccessible, or unreadable
- When rendering is attempted
- Then the app MUST show a placeholder image and SHOULD show a short reason (for example, "Missing file" or "Unreadable image").
- Given many images exist in the group
- When the view is shown
- Then the app SHOULD provide a scrollable and responsive layout for Android and Windows.

# Test Outline
1. Load a group with several images and verify all are visible with size metadata.
2. Remove or corrupt one source image and verify placeholder plus reason is displayed.
3. Verify rendering behavior on both Android and Windows form factors.

# Rationale
Visual comparison with clear fallback behavior enables reliable duplicate review even when files have changed since the JSON was created.