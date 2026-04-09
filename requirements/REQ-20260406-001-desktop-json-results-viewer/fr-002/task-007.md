# Task 007 — Manual QA: acceptance tests and checklist

Goal
- Verify the implemented navigation meets the FR-002 acceptance criteria.

Deliverable
- Execute the manual test outline and record pass/fail for each step; update `checklist.md` with results.

Test steps
1. Load a JSON file with multiple groups and confirm total count appears in summary.
2. Use `Previous`/`Next` to move between groups; verify the index updates and buttons disable at boundaries.
3. Use `Jump-to-Group` with valid input and confirm the app jumps to the requested group and the hash remains visible.
4. Use `Jump-to-Group` with invalid input and confirm validation messaging and no navigation.

Acceptance criteria
- All four test scenarios pass; any failures are recorded with steps to reproduce.

Estimate
- 30–60 minutes
