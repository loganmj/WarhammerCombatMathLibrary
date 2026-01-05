# Warhammer Combat Math Library - GitHub Copilot Instructions

## Project Overview

This is a .NET library for calculating combat math for Warhammer 40k. It provides probability analysis and combat calculations based on the game's dice rolling mechanics.

## Technology Stack

- **.NET 9.0** - Target framework
- **C#** - Primary language with implicit usings and nullable reference types enabled
- **MSTest** - Testing framework for unit tests
- **NuGet** - Package distribution platform

## Project Structure

- `WarhammerCombatMathLibrary/WarhammerCombatMathLibrary/` - Main library source code
  - `CombatMath.cs` - Core combat calculation logic
  - `Statistics.cs` - Statistical and probability functions
  - `MathUtilities.cs` - Mathematical utility functions
  - `BoundedCache.cs` - Caching implementation for performance
  - `Data/` - Data transfer objects (DTOs) and enums
- `WarhammerCombatMathLibrary/UnitTests/` - Unit test project
- `Docs/` - Documentation including game rules and mathematical explanations

## Coding Standards and Conventions

### General C# Standards

- Use **implicit usings** as enabled in the project
- Enable and follow **nullable reference types** - all nullable types should be explicitly marked with `?`
- Use **file-scoped namespaces** where appropriate
- Follow standard C# naming conventions:
  - PascalCase for class names, method names, properties
  - camelCase with underscore prefix for private fields (e.g., `_privateField`)
  - UPPER_CASE for constants
  - Suffix DTOs with `DTO` (e.g., `AttackerDTO`, `DefenderDTO`)

### Code Organization

- Use **region directives** to organize code sections:
  - `#region Constants` - For constant declarations
  - `#region Fields` - For field declarations
  - `#region Properties` - For properties
  - `#region Private Methods` - For private methods
  - `#region Public Methods` - For public methods
- Place constants at the top of classes
- Keep related functionality together within regions

### Documentation

- **Always include XML documentation comments** for:
  - All public classes, methods, properties, and fields
  - Use `<summary>` tags for descriptions
  - Use `<param>` tags for parameters
  - Use `<returns>` tags for return values
- Documentation should be clear and explain what the code does in the context of Warhammer 40k combat

### Testing

- All test classes should be in the `UnitTests` namespace
- Test class names should end with `_Test` (e.g., `CombatMath_Test`)
- Use the `[TestClass]` attribute on test classes
- Mark test classes as `sealed` when appropriate
- Use descriptive test method names that explain what is being tested
- Use readonly static test data objects for reusable test scenarios
- Test data should use object initializer syntax

### Performance Considerations

- Use caching for expensive calculations (see `BoundedCache<TKey, TValue>`)
- The library uses `BoundedCache` with a max size of 5000 for probability calculations
- Be mindful of performance in probability and statistical calculations

## Build and Test Commands

### Restore Dependencies
```bash
dotnet restore ./WarhammerCombatMathLibrary/WarhammerCombatMathLibrary.sln
```

### Build
```bash
dotnet build --configuration Release --no-restore ./WarhammerCombatMathLibrary/WarhammerCombatMathLibrary.sln
```

### Run Tests
```bash
dotnet test ./WarhammerCombatMathLibrary/UnitTests/UnitTests.csproj --no-restore --verbosity normal
```

### Create NuGet Package
```bash
dotnet pack --configuration Release --no-build --output ./nupkg ./WarhammerCombatMathLibrary/WarhammerCombatMathLibrary/WarhammerCombatMathLibrary.csproj
```

## Domain Knowledge

### Warhammer 40k Combat Mechanics

- Combat is resolved through a sequence of dice rolls (hit, wound, save, damage)
- Uses six-sided dice (D6)
- A roll of 1 is always a failure
- A roll of 6 is always a success
- Modifiers are capped at +/- 1 after combining attacker and defender modifiers
- Key stats: Weapon Skill (WS), Strength (S), Toughness (T), Armor Save (Sv), etc.
- See `Docs/40kRules.md` for detailed game mechanics
- See `Docs/Math.md` for probability and mathematical explanations

### Data Objects

- `AttackerDTO` - Represents attacking unit characteristics
- `DefenderDTO` - Represents defending unit characteristics
- `BinomialOutcome` - Represents probability distribution outcomes
- `DiceType` - Enum for different dice types (D3, D6, etc.)

## Code Change Boundaries

### What to Do

- Make only the changes explicitly requested in the issue or task
- Add appropriate XML documentation for new public members
- Add unit tests for new functionality, following existing test patterns
- Update relevant documentation in `Docs/` if game mechanics or math change
- Maintain existing code style and conventions

### What NOT to Do

- Do not refactor or optimize code unless explicitly requested
- Do not modify unrelated files or sections of code
- Do not change existing test data or test behavior unless necessary for the task
- Do not alter the public API of the library without explicit approval
- Do not introduce new dependencies without discussion
- Avoid making changes to build configuration or project settings unless required

## Collaboration

- If requirements are unclear or ambiguous, ask for clarification before implementing
- When dealing with probability calculations, verify mathematical correctness
- Consider performance implications for changes to hot paths like `Statistics` or `CombatMath`
- Ensure backward compatibility for public APIs in this NuGet package
- Follow the existing patterns for caching and performance optimization
