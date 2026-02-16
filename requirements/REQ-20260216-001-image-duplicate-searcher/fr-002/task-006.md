# Task 006 — Implement Extension Filtering

# Description
Add filtering logic in the ScanDirectories method to only include files with supported extensions, using case-insensitive comparison.

# Deliverable
Updated ImageProcessor.cs with extension filtering.

# Dependencies
Task 005

# Implementation Notes
Check file extension against SupportedFormats list. Use Path.GetExtension and string comparison with StringComparison.OrdinalIgnoreCase.