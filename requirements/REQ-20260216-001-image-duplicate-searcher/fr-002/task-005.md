# Task 005 — Implement Directory Enumeration

# Description
Implement the ScanDirectories method in ImageProcessor to enumerate files in each configured directory using Directory.EnumerateFiles.

# Deliverable
Updated ImageProcessor.cs with directory enumeration logic.

# Dependencies
Task 004

# Implementation Notes
Use Directory.EnumerateFiles for each directory in ImageDirectories. Collect all file paths into a list. Handle cases where directories don't exist.