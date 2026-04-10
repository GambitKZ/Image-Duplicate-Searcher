# Android Platform Adapter Notes

Date: 2026-04-10

Summary
- `AndroidPlatformFileService` provides a picker-based flow and attempts to request read permissions before file access. When the platform returns a content URI or otherwise does not expose a filesystem path, the adapter copies the picked file into the app cache and returns that cache path to callers.

Important caveats
- When a copy is returned (cache path), deleting that path will not delete the original file selected by the user — deletion of the original requires an Android-specific approach (DocumentFile / ContentResolver) and stronger permissions.
- For removal operations that need to delete the original image on disk, implement an Android-specific deletion flow that uses persisted SAF URIs or requests the user to grant access to the parent directory.

Recommendations
- Use the adapter to select and read JSON results; perform deletions with `IImageRemovalService` but note that `ImageRemovalService` performs `File.Delete(path)` and will fail if path is not the original filesystem path.
- Consider extending the adapter to return a small DTO: `{ Path, IsTemporaryCopy, OriginalUri }` so removal logic can branch appropriately on Android.
