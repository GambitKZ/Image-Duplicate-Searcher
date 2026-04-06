## Task 003 — Parse and validate Supported Formats in ImageDuplicationOptions

Goal

- Ensure `Supported Formats` edited in the UI is parsed into a `string[]` suitable for runtime use and validated.

Deliverable

- Add or extend helper on `ImageDuplicationOptions`:
  - Parse `"jpg;png;bmp"` into `[".jpg",".png",".bmp"]`
  - Trim whitespace, ensure leading dot on extensions, reject empty tokens
  - Expose a parsed property or method consumed by services

Estimate

- 30–60 minutes

Dependencies

- Task 001
