# REQ-20260216-001:FR-003 — Generate In-Memory Thumbnails for Images

# User Story
As a user, I want the application to create small thumbnails of images so that hash calculation is efficient.

# Acceptance Criteria (Given / When / Then)
- Given an image file is provided
- When the thumbnail is generated
- Then a JPEG thumbnail of 64x64 pixels MUST be created in memory

# Test Outline
1. Provide a sample image file.
2. Call thumbnail generation.
3. Verify the output is a 64x64 JPEG image in memory.

# Rationale
Thumbnails enable fast processing and comparison without loading full-size images into memory.