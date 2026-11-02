# ImageDuplicateSearcher - AI Agent Instructions

## Technology Stack & Versions

### .NET Ecosystem
- **.NET Core 10** - Runtime and framework for all applications
- **C# 14** - Primary programming language
- **.NET MAUI** - Cross-platform UI framework for Desktop application

### Application Architecture
- **Console app** (`ImageDuplicateSearcher.Console`) - CLI interface with `appsettings.json` configuration
- **Desktop app** (`ImageDuplicationSearcher.Desktop`) - MAUI-based graphical interface
- **Application layer** (`ImageDuplicateSearcher.Application`) - Shared business logic, services, and models

### Supporting Technologies
- **FluentValidation** - Input validation and business rule enforcement
- **Dependency Injection** - Built-in .NET DI for service composition

## Architecture Overview

Three-tier application for detecting and managing duplicate images:
- **Services layer**: `IImageProcessor`, `IDuplicateNavigator`, `IImageRemovalService`, `IResultsLoader`, `IImageDisplayManager`
- **Models layer**: Domain models (`ImageModel`, `DuplicateSearchResult`) and result objects (`RemovalResult`, `ImageDisplayResult`)
- **UI adapters**: Console reporter and MAUI desktop interface via `IUiReporter` and `IImageDisplayManager`

## When Assisting with ImageDuplicateSearcher

1. Reference **.NET Core 10** and **C# 14** compatibility requirements
2. For C# code standards, apply rules from the **dotnet-writing skill**
3. For **Desktop UI**: Use .NET MAUI following patterns in `ImageDuplicationSearcher.Desktop/`; review `MauiProgram.cs` for DI configuration
4. For **Console UI**: Use `IUiReporter` abstraction; follow `ConsoleReporter` pattern
5. Reference existing service interfaces in `Interfaces/` when implementing new features
6. Use result objects (`RemovalResult`, `ImageDisplayResult`) for operation outcomes