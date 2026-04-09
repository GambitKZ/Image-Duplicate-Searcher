# Task 004 — Add navigation controls to `MainPage.xaml`

Goal
- Add UI controls to support group-based navigation: summary, Previous/Next, Jump-to-Group entry, Go button, and visible hash for the active group.

Deliverable
- Update `src/ImageDuplicateSearcher.Desktop/MainPage.xaml` to include a navigation section (suggest below the existing separator) with:
  - `Label` for summary (e.g. `SummaryLabel`) that shows `"X / N"`.
  - `Button` `PrevButton` and `NextButton` for step navigation.
  - `Entry` `GroupEntry` for numeric input and `Button` `GoButton` to trigger jump.
  - `Label` `GroupHashLabel` to display the active group's hash value.

Acceptance criteria
- Controls exist with `x:Name` attributes so code-behind can reference them. Layout should be responsive and not break existing controls.

Estimate
- 45–90 minutes

Notes
- Reuse existing `ErrorLabel` for validation messages or add a dedicated small validation label.
