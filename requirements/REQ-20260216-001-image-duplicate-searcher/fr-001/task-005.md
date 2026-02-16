# Task 005 — Implement Configuration Validation

# Description
Add validation logic in ConfigurationManager to check that required settings (e.g., at least one image directory) are present and valid on startup.

# Deliverable
Validation code in ConfigurationManager, throwing exceptions if invalid.

# Dependencies
Task 004 (ConfigurationManager exists)

# Implementation Notes
Validate on construction or via a Validate method. Handle missing or invalid paths.