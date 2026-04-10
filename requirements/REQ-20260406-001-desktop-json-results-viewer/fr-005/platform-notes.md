# Platform Notes — Android Limitations & Recommended Fallbacks

Date: 2026-04-10

Purpose
- Explain Android-specific storage/permission limitations relevant to the Desktop JSON-results viewer and recommend safe, testable fallbacks so the Windows and Android experiences remain functionally comparable where possible.

Summary of Constraints
- Android storage access changed substantially across API levels:
  - Pre-Android 10 (API < 29): apps could request `READ_EXTERNAL_STORAGE`/`WRITE_EXTERNAL_STORAGE` and access paths directly.
  - Android 10/11 (API 29/30): Scoped storage introduced; direct file-path access is restricted; apps should prefer SAF or media APIs.
  - Android 13+ (API 33+): New granular media permissions (`READ_MEDIA_IMAGES`); legacy `READ_EXTERNAL_STORAGE` is deprecated on some targets.

Implications for this app
- File picking: Use SAF / MAUI `FilePicker.PickAsync()` as primary flow. File picker grants the app access to the selected file without requiring broad storage permissions.
- Path availability: `FilePicker` may return a content URI without a usable filesystem path (`FullPath` may be null). Relying on `File.Delete(path)` is therefore unreliable on Android.
- Deletions: Deleting the original file selected via SAF typically requires using persisted URIs and Android `ContentResolver`/`DocumentFile` APIs or requesting broad management permissions (not recommended).

Current implementation caveats
- `ImageRemovalService.RemoveImageAsync(string path)` executes `File.Delete(path)`. This works on Windows and on Android only when the app has a real filesystem path and permissions. When the adapter returns a temporary cache copy path (as implemented), `RemoveImageAsync` will delete the temporary cache copy but not the original file selected by the user.

Recommended changes & platform adapters
- Keep `ImageRemovalService` as the cross-platform file-deletion API, but extend the Android adapter to support two flows:
  1. If the app has an original persisted SAF URI for the file, call an Android-specific deletion routine that uses `ContentResolver` / `DocumentFile` to remove the original.
  2. If only a temporary cache copy exists, delete the cache copy and inform the user the original file was not removed.
- Consider changing the platform picker adapter to return a small DTO instead of a raw string path:

  {
    "Path": "...local-or-cache-path...",
    "IsTemporaryCopy": true|false,
    "OriginalUri": "content://..." // optional
  }

UX recommendations
- When deleting on Android, present a confirmation dialog that explains whether the deletion will remove the original file or only the local cached copy.
- If original-file deletion is not possible, offer an explicit user flow: "Open file in system picker" so the user can delete from their file manager.

Testing guidance
- Manual tests to cover:
  - Android API 26 (legacy behavior) — verify pick + delete by path.
  - Android API 29/30 — verify picker returns content URIs and fallback cache-copy behavior.
  - Android API 33+ — verify `READ_MEDIA_IMAGES` behavior and whether picker returns a path vs content URI.

Implementation notes
- Implement Android deletion using platform-specific code in `AndroidPlatformFileService` or a separate `IPlatformDeletionService`.
- Avoid requesting `MANAGE_EXTERNAL_STORAGE` unless absolutely required — it triggers Play Store policy scrutiny.

Next steps
- Update `IPlatformFileService` to return an enriched result (DTO) rather than a raw path if removal-by-original-URI will be supported.
- Update `ImageRemovalService` or add an Android-specific deletion helper to reconcile SAF URIs and `File.Delete` semantics.
