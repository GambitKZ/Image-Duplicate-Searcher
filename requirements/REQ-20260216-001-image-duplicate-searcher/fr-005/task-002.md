# Task 002 — Implement DuplicateGrouper Class

# Description
Implement the DuplicateGrouper class that implements IDuplicateGrouper, using a Dictionary to efficiently group images by their computed hash.

# Deliverable
DuplicateGrouper.cs file in the src/Services/ folder.

# Dependencies
IDuplicateGrouper interface (from Task 001)

# Implementation Notes
- Use Dictionary<ulong, List<string>> for grouping.
- Handle hash collisions by adding to the existing list.
- Ensure efficient addition of images to groups.