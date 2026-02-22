# REQ-20260216-001:FR-002 — Scan Configured Directory for Image Files

# User Story
As a user, I want the application to scan a specified directory for image files so that it can process them for duplicates.

# Acceptance Criteria (Given / When / Then)
- Given a configured directory exists with image files
- When the application scans the directory
- Then supported image files (JPEG, JPG, PNG, BMP) MUST be identified and listed, and unsupported files SHOULD be ignored

# Test Outline
1. Create a test directory with supported and unsupported files.
2. Configure the app to scan that directory.
3. Run the scan and verify only supported images are processed.

# Rationale
Scanning ensures only relevant image files are considered for duplicate detection, improving efficiency and accuracy.