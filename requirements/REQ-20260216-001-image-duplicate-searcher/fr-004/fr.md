# REQ-20260216-001:FR-004 — Compute Perceptual Hash from Thumbnails

# User Story
As a user, I want the application to compute a perceptual hash for each thumbnail so that similar images can be identified.

# Acceptance Criteria (Given / When / Then)
- Given a thumbnail image is available
- When the perceptual hash is computed
- Then a consistent hash value MUST be generated based on the image content

# Test Outline
1. Generate a thumbnail from an image.
2. Compute the perceptual hash.
3. Verify the hash is a valid, consistent value.

# Rationale
Perceptual hashing enables detection of duplicate or similar images based on visual content rather than exact matches.