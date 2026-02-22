# Task 002 — Implement Directory Scanning Logic

# Description
Implement the ScanDirectory method in ImageProcessor to scan the configured directory and filter image files by supported extensions.

# Deliverable
ScanDirectory method that uses Directory.EnumerateFiles and filters based on SupportedFormats.

# Dependencies
Task 001

# Implementation Notes
Use case-insensitive comparison for extensions. Ensure the directory exists before scanning.