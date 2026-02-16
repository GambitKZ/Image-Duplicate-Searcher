# Task 007 — Register IImageProcessor in DI

# Description
Register IImageProcessor as a scoped service in the DI container in Program.cs.

# Deliverable
Updated Program.cs with service registration.

# Dependencies
Task 004

# Implementation Notes
Add services.AddScoped<IImageProcessor, ImageProcessor>(); in the ConfigureServices lambda.