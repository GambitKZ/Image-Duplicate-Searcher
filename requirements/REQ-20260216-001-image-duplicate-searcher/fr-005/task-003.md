# Task 003 — Integrate Duplicate Grouper into Application Flow

# Description
Update Program.cs to compute perceptual hashes for all scanned images, group them using DuplicateGrouper, and display the grouped results for verification.

# Deliverable
Modified Program.cs file with hash computation and grouping logic.

# Dependencies
DuplicateGrouper class (from Task 002), ImageProcessor.ComputePerceptualHash method

# Implementation Notes
- After scanning images, compute hash for each using ImageProcessor.
- Collect IEnumerable<(string, ulong)>.
- Use DuplicateGrouper.GroupByHash to get the groups.
- Print the groups to console for acceptance criteria verification.