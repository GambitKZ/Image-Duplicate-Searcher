# Task 006 — Add responsive image canvas (XAML)

Goal

- Add a scrollable, responsive grid/collection area to `MainPage.xaml` to render all images for the active group.

Deliverables

- Updated `src/ImageDuplicationSearcher.Desktop/MainPage.xaml`

Description

- Add a `CollectionView` or `FlexLayout` inside the existing page that binds to a collection of image tiles.
- Item template shows: image (bound `ImageSource`), file path, and `sizeMB` label.
- Ensure layout uses 2–4 columns depending on screen width and is scrollable.
- Include placeholder UI for missing/unreadable images; do not implement Remove button behavior (defer to FR-004).

Acceptance Criteria

- XAML compiles and displays the collection area (bindings can be stubbed until Task 007).
- Layout is responsive on tablet/phone/desktop widths.

Estimated time

- 1 hour

Dependencies

- Task 002 (model)
