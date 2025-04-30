---
layout: docs
title: UnambitiousFx.Core
nav_order: 1
---

# UnambitiousFx.Core

UnambitiousFx.Core is the foundation library of the UnambitiousFx project, providing essential utilities and extensions that simplify common programming tasks in C#.

## Installation

Install the UnambitiousFx.Core package via NuGet:

```bash
Install-Package UnambitiousFx.Core
```

## Features

### String Extensions

UnambitiousFx.Core provides a set of useful string extension methods:

```csharp
using UnambitiousFx.Core.Extensions;

// Example string extensions
string text = "Hello, World!";
bool containsHello = text.ContainsIgnoreCase("hello"); // true
string truncated = text.Truncate(5); // "Hello"
string slugified = text.ToSlug(); // "hello-world"
```

### Collection Helpers

Work with collections more efficiently:

```csharp
using UnambitiousFx.Core.Collections;

// Example collection operations
var items = new List<string> { "apple", "banana", "cherry" };
var batches = items.Batch(2); // Creates batches of 2 items
var shuffled = items.Shuffle(); // Returns a shuffled copy of the list
```

### Date and Time Utilities

Simplify date and time operations:

```csharp
using UnambitiousFx.Core.Time;

// Example date/time operations
var tomorrow = DateTimeHelper.Tomorrow();
var isWeekend = DateTimeHelper.IsWeekend(DateTime.Now);
var quarter = DateTimeHelper.GetQuarter(DateTime.Now);
```

### Guard Clauses

Validate method parameters easily:

```csharp
using UnambitiousFx.Core.Guards;

public void ProcessData(string input, int count)
{
    Guard.AgainstNull(input, nameof(input));
    Guard.AgainstNegativeOrZero(count, nameof(count));

    // Method implementation...
}
```

## API Reference

For detailed API documentation, please visit the [UnambitiousFx.Core API Reference](/docs/core/api/).

## Examples

Check out our [examples repository](https://github.com/UnambitiousFx/examples) for more examples of using UnambitiousFx.Core.
