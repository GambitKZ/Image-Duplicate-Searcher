# Task 007 — Add Remove button to tile DataTemplate in `MainPage.xaml`

Goal
- Expose a visible Remove action in the tile UI that calls the view-model's `RemoveCommand`.

Deliverable
- Edit `src/ImageDuplicationSearcher.Desktop/MainPage.xaml` DataTemplate for `ImageCollectionView` to add a `Button` with `Command="{Binding RemoveCommand}"` and accessible text (e.g., "Remove").
- Style the button to be small and placed near the image (e.g., below or overlay corner).

Acceptance Criteria
- Remove button is visible for tiles and binds to `RemoveCommand` without XAML binding errors.

Estimated Effort
- 15–30 minutes

Dependencies
- Task 005

Notes
- Keep accessibility in mind (AutomationProperties.Name) and avoid blocking the image display.