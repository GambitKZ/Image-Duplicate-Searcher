# REQ-20260216-001:FR-002 — Scan Configured Directories for Image Files

# User Story
As a user, I want the application to scan specified directories for image files so that it can process them for duplicates.

# Acceptance Criteria (Given / When / Then)
- Given configured directories exist with image files
- When the application scans the directories
- Then supported image files (JPEG, JPG, PNG, BMP) MUST be identified and listed, and unsupported files SHOULD be ignored

# Test Outline
1. Create test directories with supported and unsupported files.
2. Configure the app to scan those directories.
3. Run the scan and verify only supported images are processed.

# Rationale
Scanning ensures only relevant image files are considered for duplicate detection, improving efficiency and accuracy.