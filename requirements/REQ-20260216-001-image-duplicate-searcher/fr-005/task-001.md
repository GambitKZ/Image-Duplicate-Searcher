# Task 001 — Create IDuplicateGrouper Interface

# Description
Create an interface IDuplicateGrouper in the Services folder to define the contract for grouping duplicate images by hash.

# Deliverable
IDuplicateGrouper.cs file in the src/Services/ folder with the GroupByHash method.

# Dependencies
None

# Implementation Notes
- Use PascalCase for naming.
- Include XML documentation comments.
- The method should take IEnumerable<(string FilePath, ulong Hash)> and return Dictionary<ulong, List<string>>.