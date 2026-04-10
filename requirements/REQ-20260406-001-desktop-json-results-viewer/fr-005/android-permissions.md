# Android Storage Permissions & Runtime Guidance

Date: 2026-04-10

Summary
- This note documents the Android permissions added to the manifest and describes recommended runtime handling and fallbacks for scoped-storage constraints.

Manifest changes (applied)
- Added the following permissions to `Platforms/Android/AndroidManifest.xml`:
  - `android.permission.READ_EXTERNAL_STORAGE`
  - `android.permission.WRITE_EXTERNAL_STORAGE`
  - `android.permission.READ_MEDIA_IMAGES` (Android 13+)

Guidance
- Prefer the Storage Access Framework (SAF) / `FilePicker.PickAsync` for selecting files — it avoids needing broad storage permissions and works across Android versions.
- If the app needs direct file-path access (e.g., to delete image files by path), implement an Android adapter that:
  1. Checks runtime permission status.
 2. If not granted, requests the minimal scoped permission (e.g., `READ_EXTERNAL_STORAGE`) using MAUI's `Permissions` API or Android `ActivityCompat.RequestPermissions`.
 3. For Android 11+ consider using `MediaStore` APIs or `MANAGE_EXTERNAL_STORAGE` only if absolutely necessary (this requires policy approval and is not recommended).

Sample MAUI permission check (illustrative)

```csharp
using Microsoft.Maui.ApplicationModel;

public static async Task<bool> EnsureReadPermissionAsync()
{
    var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
    if (status != PermissionStatus.Granted)
    {
        status = await Permissions.RequestAsync<Permissions.StorageRead>();
    }
    return status == PermissionStatus.Granted;
}
```

Notes on deletion
- Deleting files by path on newer Android versions may fail even with `READ_EXTERNAL_STORAGE`; prefer SAF URIs or request the user to open the file via File Picker and use the returned `FullPath` or persisted URI.

Fallbacks & UX
- If permission is denied, present a clear message with a button to open app settings, or offer a safer flow: "Open file via picker" so the user grants access only to selected files.
- Document platform limitations in the `platform-notes.md` file (Task 008).

Next steps (implementation items for Task 006 / Task 008)
- Implement `AndroidPlatformFileService` to encapsulate the permission checks and SAF/file-picking logic.
- Add end-to-end tests or manual test steps to verify file deletion and file selection on Android API levels 26, 29, 30, 31+.
