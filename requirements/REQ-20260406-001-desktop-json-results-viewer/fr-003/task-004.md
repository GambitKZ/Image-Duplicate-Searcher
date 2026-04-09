# Task 004 — Add embedded placeholder image resource

Goal

- Provide a deterministic placeholder image used when image files are missing or unreadable.

Deliverables

- `src/ImageDuplicateSearcher.Application/Resources/placeholder.png`
- Project metadata updated so the image is available as an embedded resource (if preferred) or accessible from the service.

Description

- Add a simple 256x256 PNG placeholder (icon-style) to Application resources.
- Ensure `ImageDisplayManager` can load the placeholder bytes from the assembly resource stream.

Acceptance Criteria

- Placeholder bytes are returned by the Application service when needed.

Estimated time

- 20–30 minutes

Dependencies

- Task 003 (service implementation)
