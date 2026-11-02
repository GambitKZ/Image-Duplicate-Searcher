---
name: dotnet-writing
description: 'Write correct, maintainable C# code following .NET Core 10 standards. Use when: implementing C# features; writing or reviewing service layers, interfaces, models, or tests; applying design patterns; ensuring proper validation, error handling, and nullable reference types; structuring projects with domain-driven design.'
argument-hint: 'Describe the C# task: "implement a service", "write unit tests", "create a model", etc.'
---

# .NET C# Development

## When to Use This Skill

- **Writing C# code** for .NET Core 10 applications (Console, Desktop, Web)
- **Implementing services, interfaces, and models** with proper separation of concerns
- **Applying naming conventions, formatting, and code style** from the project standards
- **Ensuring validation and error handling** using FluentValidation, data annotations, and global middleware
- **Writing unit and integration tests** with clear, maintainable test methods
- **Structuring projects** with feature folders or domain-driven design principles
- **Handling nullable reference types** correctly with proper null checks
- **Reviewing code changes** to ensure maintainability and best practices

## Code Standards

### C# Version & Language Features
- Always use **C# 14** (latest version)
- Leverage modern features: pattern matching, switch expressions, file-scoped namespaces, required properties
- Use `nameof()` instead of string literals for member references
- Apply nullable reference types consistently (`is null`, `is not null`)

### Naming Conventions
- **PascalCase**: Component names, method names, public members, interface implementations
- **camelCase**: Private fields, local variables, parameters
- **Interface prefix**: `I` (e.g., `IUserService`, `IImageProcessor`)

### Formatting & Structure
- Insert a newline before opening braces for all code blocks (`if`, `for`, `while`, `foreach`, `using`, `try`)
- Use file-scoped namespace declarations: `namespace MyApp.Services;` (not nested braces)
- Apply `.editorconfig` formatting rules consistently
- Ensure final return statement is on its own line
- Single-line using directives

### Documentation
- **Required for all public APIs**: XML doc comments (`///`)
- Include `<summary>`, `<param>`, `<returns>`, and `<exception>` tags as appropriate
- Add `<example>` and `<code>` sections for complex methods
- Write clear comments explaining *why* design decisions were made

## Architecture & Design

### Project Organization
- **Separation of concerns**: Models, Services, Interfaces, Data Access layers
- **Interfaces first**: Place in dedicated `Interfaces/` folder
- **Dependency injection**: Configure in `Program.cs` (ASP.NET Core)
- **Settings management**: Use `IOptions<T>` for configuration (e.g., `ImageDuplicationOptions`)
- **Domain-driven design**: Organize by feature folders or business domains

### Service Layer
- Services implement interfaces for loose coupling
- Inject dependencies via constructor
- Validate inputs at entry points
- Handle business logic; keep models dumb (data holders only)

### Models & Entities
- Models represent domain concepts or API contracts
- Use data annotations or FluentValidation for validation rules
- Keep models focused; avoid bloated classes

## Nullable Reference Types & Null Safety

- **Declare non-nullable by default**: `public string Name { get; set; }` (required)
- **Check for null at entry points**: Controller actions, public method entry points
- **Use proper operators**: `is null`, `is not null` (never `== null`)
- **Trust the type system**: Don't add redundant null checks when the type guarantees non-null

## Validation & Error Handling

### Input Validation
- Use **data annotations** for simple rules: `[Required]`, `[Range]`, `[EmailAddress]`
- Use **FluentValidation** for complex validation logic
- Validate at the boundary (controllers, services)

### Exception Handling
- Create custom exception types for domain errors
- Use global exception handling middleware (ASP.NET Core)
- Implement **RFC 7807 Problem Details** for standardized error responses
- Log exceptions with context; include meaningful error messages for users

### Result Objects
- Return result objects (e.g., `RemovalResult`, `DuplicateSearchResult`) for operations that may fail
- Include success flag, data, and error information in results
- Avoid throwing exceptions for expected failures; use result patterns

## Performance Optimizations

### Source-Generated Logging
- **Use `ILogger<T>` with structured logging** to enable compile-time logging code generation
- **Avoid boxing**: Use strongly-typed log parameters instead of passing `object[]`
- **Use `LoggerMessageAttribute`** to define log messages as static methods:
  ```csharp
  [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Processing image {ImagePath}")]
  partial void LogProcessingImage(string imagePath);
  ```
- **Benefits**: Zero allocation for log messages that don't match the current log level, better performance
- Inject `ILogger<ServiceName>` in your services and use typed log methods

### Source-Generated Regular Expressions
- **Use `[GeneratedRegex]` instead of `Regex.Compile()` or field-based regex instances**
- **Avoids allocations at runtime** and leverages compile-time code generation
- **Example**:
  ```csharp
  [GeneratedRegex(@"(?<ImagePath>.+?)\.jpg$", RegexOptions.IgnoreCase)]
  private static partial Regex ImageFileRegex();
  
  // Use it:
  if (ImageFileRegex().IsMatch(filePath))
  {
      // Process image
  }
  ```
- **Benefits**: One-time generated code, no regex compilation at runtime, reduced memory pressure
- Define generated regex methods as `private static partial` in your classes

## Testing

### Test Structure
- Organize tests by feature or service being tested
- Use clear, descriptive test method names (e.g., `MarkDuplicateForRemoval_WithValidImage_SuccessfullyMarked`)
- Follow **Arrange-Act-Assert** pattern (without explicit comments)
- Test critical paths: happy path, edge cases, error scenarios

### Mocking
- Mock dependencies for unit tests
- Mock database calls, external APIs, UI interactions
- Keep mocks simple; mock only what's necessary

## Step-by-Step Workflow

### When Implementing a New Service

1. **Define the interface** in `Interfaces/I<ServiceName>.cs`
2. **List dependencies** the service needs (to inject)
3. **Write the implementation** in `Services/<ServiceName>.cs`
4. **Add XML documentation** for all public members
5. **Implement validation** for inputs (at method entry points)
6. **Handle exceptions** with custom exception types or result objects
7. **Register in DI container** (in `Program.cs` for console; `MauiProgram.cs` for Desktop)
8. **Write unit tests** covering happy path and error cases

### When Writing a Model

1. **Define properties** with appropriate types (non-nullable by default)
2. **Add data annotations** for validation: `[Required]`, `[Range]`, etc.
3. **Add XML comments** documenting the purpose of each property
4. **Avoid business logic** in models (models are data containers)
5. **Create companion interfaces** if the model needs to be injected as a dependency

### When Creating a New Feature

1. **Analyze requirements**: What does the feature do?
2. **Design the interface**: What are the entry points?
3. **Implement the service**: Follow separation of concerns
4. **Write tests first** (test-driven development recommended)
5. **Handle errors gracefully**: Use validation and result objects
6. **Document with XML**: Ensure public APIs are self-documenting

## Quick Reference

| Task | Pattern |
|------|---------|
| Validate input | `[Required]` attribute or FluentValidation |
| Handle failure | Return `Result<T>` object, not exception |
| Check null | Use `is null` or `is not null` |
| Refer to names | Use `nameof(Property)` |
| Async operations | Use `async Task<T>` and `.ConfigureAwait(false)` |
| Configuration | Inject `IOptions<T>` into constructor |
| Dependency injection | Register in `Program.cs`; inject via constructor |
| Report UI events | Use `IUiReporter` or similar interface |
| Logging without boxing | Use `ILogger<T>` with `[LoggerMessage]` (source-generated) |
| Regex matching | Use `[GeneratedRegex]` instead of `Regex.Compile()` |

## Resources

- `.editorconfig`: Formatting and style rules for the project