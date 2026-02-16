# REQ-20260216-001:FR-002 — Scan Configured Directories for Image Files

# Design Considerations
- Scan only top-level directories, no subfolders
- Filter by file extensions: .jpeg, .jpg, .png, .bmp (configurable)
- Handle large numbers of files efficiently

# Data Flow
1. Get directory paths from configuration
2. For each directory, enumerate files
3. Filter by supported extensions
4. Collect list of image file paths

# Affected Components (Projects, Services, Classes)
- Image Processor

# Dependencies
- System.IO for file enumeration
- Configuration for supported formats

# Implementation Steps
1. Read directory paths from config
2. Use Directory.EnumerateFiles with filter
3. Apply extension check
4. Return list of valid image paths