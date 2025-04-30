---
layout: docs
title: UnambitiousFx.Data
nav_order: 2
---

# UnambitiousFx.Data

UnambitiousFx.Data is a library focused on simplifying data access and manipulation in C# applications. It provides a clean, straightforward API for working with databases, data transformation, and serialization.

## Installation

Install the UnambitiousFx.Data package via NuGet:

```bash
Install-Package UnambitiousFx.Data
```

## Features

### Database Access

UnambitiousFx.Data provides a simple database access layer:

```csharp
using UnambitiousFx.Data.Database;

// Example database operations
using (var db = new DatabaseConnection("connection_string"))
{
    // Query data
    var users = db.Query<User>("SELECT * FROM Users WHERE Active = @Active", new { Active = true });

    // Execute commands
    int affected = db.Execute("UPDATE Users SET LastLogin = @Now WHERE Id = @Id", 
                             new { Now = DateTime.UtcNow, Id = 123 });
}
```

### Data Mapping

Map between different data models easily:

```csharp
using UnambitiousFx.Data.Mapping;

// Define your models
public class UserDto { public int Id; public string Name; public string Email; }
public class UserEntity { public int UserId; public string FullName; public string EmailAddress; }

// Configure mapping
var mapper = new DataMapper();
mapper.CreateMap<UserEntity, UserDto>()
    .MapProperty(src => src.UserId, dest => dest.Id)
    .MapProperty(src => src.FullName, dest => dest.Name)
    .MapProperty(src => src.EmailAddress, dest => dest.Email);

// Use the mapper
UserEntity entity = GetUserFromDatabase();
UserDto dto = mapper.Map<UserDto>(entity);
```

### Serialization

Serialize and deserialize data in various formats:

```csharp
using UnambitiousFx.Data.Serialization;

// JSON serialization
var user = new User { Id = 1, Name = "John Doe" };
string json = JsonSerializer.Serialize(user);
User deserialized = JsonSerializer.Deserialize<User>(json);

// CSV serialization
var users = GetUsers();
string csv = CsvSerializer.Serialize(users);
IEnumerable<User> deserializedUsers = CsvSerializer.Deserialize<User>(csv);
```

### Data Validation

Validate data models with a fluent API:

```csharp
using UnambitiousFx.Data.Validation;

public class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name).NotEmpty().MaxLength(100);
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.Age).GreaterThan(0).LessThan(120);
    }
}

// Using the validator
var validator = new UserValidator();
ValidationResult result = validator.Validate(user);
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error.ErrorMessage);
    }
}
```

## API Reference

For detailed API documentation, please visit the [UnambitiousFx.Data API Reference](/docs/data/api/).

## Examples

Check out our [examples repository](https://github.com/UnambitiousFx/examples) for more examples of using UnambitiousFx.Data.
