---
layout: default
title: Request Validation
parent: Mediator
nav_order: 6
---

# Request Validation

UnambitiousFx.Mediator provides a built-in validation system that allows you to validate requests before they reach
their handlers. This keeps your handlers focused on business logic while ensuring that all requests meet your validation
requirements.

The validation system is extensible and supports both custom validators and integration with third-party validation
libraries like FluentValidation.

## Table of Contents

- [Adding the Validation Behavior](#adding-the-validation-behavior)
- [Creating Custom Validators](#creating-custom-validators)
- [Using FluentValidation](#using-fluentvalidation)
- [Multiple Validators](#multiple-validators)
- [Error Handling](#error-handling)

## Adding the Validation Behavior

Before you can use validators, you need to register the `RequestValidationBehavior` in your mediator configuration. This
behavior intercepts requests and runs all registered validators before the handler executes.

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Pipelines;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        services.AddMediator(config => {
            // Register the validation behavior
            config.RegisterRequestPipelineBehavior<RequestValidationBehavior<,>>();
            
            // Register your validators (see sections below)
            // config.AddValidator<CreateUserValidator, CreateUserCommand>();
            
            // Register other components...
        });
        
        return services;
    }
}
```

**Important**: Register the validation behavior early in the pipeline, typically before other behaviors like caching or
transaction management.

## Creating Custom Validators

Custom validators are simple to implement. They implement the `IRequestValidator<TRequest>` interface and return a
`Result` indicating success or failure.

### Step 1: Define Your Request

```csharp
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Core.Results;

public record CreateUserCommand(string Email, string Name, int Age) : IRequest<Result<Guid>>;
```

### Step 2: Create a Validator

```csharp
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Core.Results;

public class CreateUserValidator : IRequestValidator<CreateUserCommand>
{
    public ValueTask<Result> ValidateAsync(
        CreateUserCommand request, 
        CancellationToken cancellationToken = default)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(request.Email))
            return ValueTask.FromResult(Result.Failure("Email is required"));
        
        if (!request.Email.Contains('@'))
            return ValueTask.FromResult(Result.Failure("Email must be a valid email address"));
        
        // Validate name
        if (string.IsNullOrWhiteSpace(request.Name))
            return ValueTask.FromResult(Result.Failure("Name is required"));
        
        if (request.Name.Length < 2)
            return ValueTask.FromResult(Result.Failure("Name must be at least 2 characters"));
        
        // Validate age
        if (request.Age < 18)
            return ValueTask.FromResult(Result.Failure("User must be at least 18 years old"));
        
        if (request.Age > 120)
            return ValueTask.FromResult(Result.Failure("Age must be realistic"));
        
        // All validations passed
        return ValueTask.FromResult(Result.Success());
    }
}
```

### Step 3: Register the Validator

```csharp
services.AddMediator(config => {
    // Register the validation behavior
    config.RegisterRequestPipelineBehavior<RequestValidationBehavior<,>>();
    
    // Register your custom validator
    config.AddValidator<CreateUserValidator, CreateUserCommand>();
});
```

### Advanced Custom Validator with Dependencies

Validators can have dependencies injected through their constructor:

```csharp
public class CreateUserValidator : IRequestValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUserValidator> _logger;

    public CreateUserValidator(
        IUserRepository userRepository, 
        ILogger<CreateUserValidator> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async ValueTask<Result> ValidateAsync(
        CreateUserCommand request, 
        CancellationToken cancellationToken = default)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.Email))
            return Result.Failure("Email is required");
        
        // Async validation with repository
        var emailExists = await _userRepository.EmailExistsAsync(request.Email, cancellationToken);
        if (emailExists)
            return Result.Failure("Email is already in use");
        
        _logger.LogInformation("Validation passed for user with email {Email}", request.Email);
        
        return Result.Success();
    }
}
```

## Using FluentValidation

FluentValidation is a popular .NET validation library. You can integrate it with UnambitiousFx.Mediator by creating an
adapter.

### Step 1: Install FluentValidation

```bash
dotnet add package FluentValidation
```

### Step 2: Create the FluentValidation Adapter

Create a reusable adapter that bridges FluentValidation with the mediator's validation system:

```csharp
using FluentValidation;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Core.Results;

public class FluentValidationAdapter<TRequest> : IRequestValidator<TRequest>
{
    private readonly IValidator<TRequest> _validator;

    public FluentValidationAdapter(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async ValueTask<Result> ValidateAsync(
        TRequest request, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new Error(e.ErrorMessage))
                .ToArray();
            
            return Result.Failure(errors);
        }

        return Result.Success();
    }
}
```

### Step 3: Create a FluentValidation Validator

```csharp
using FluentValidation;

public class CreateUserFluentValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserFluentValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters");
        
        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("User must be at least 18 years old")
            .LessThanOrEqualTo(120).WithMessage("Age must be realistic");
    }
}
```

### Step 4: Register FluentValidation

You have two options for registration:

**Option A: Manual Registration**

```csharp
services.AddMediator(config => {
    // Register the validation behavior
    config.RegisterRequestPipelineBehavior<RequestValidationBehavior<,>>();
});

// Register FluentValidation validator
services.AddScoped<IValidator<CreateUserCommand>, CreateUserFluentValidator>();

// Register the adapter as the mediator validator
services.AddScoped<IRequestValidator<CreateUserCommand>>(sp =>
{
    var fluentValidator = sp.GetRequiredService<IValidator<CreateUserCommand>>();
    return new FluentValidationAdapter<CreateUserCommand>(fluentValidator);
});
```

**Option B: Extension Method (Recommended)**

Create a helper extension method for cleaner registration:

```csharp
public static class FluentValidationExtensions
{
    public static IMediatorConfig AddFluentValidator<TValidator, TRequest>(
        this IMediatorConfig config)
        where TValidator : class, IValidator<TRequest>
        where TRequest : IRequest
    {
        // This would need to be added to the service collection directly
        // You may need to access the IServiceCollection through the config
        return config;
    }
}
```

### Step 5: Advanced FluentValidation Features

FluentValidation supports many advanced features:

```csharp
public class CreateUserFluentValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserFluentValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MustAsync(BeUniqueEmail).WithMessage("Email is already in use");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name must contain only letters and spaces");
        
        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("User must be at least 18 years old")
            .LessThanOrEqualTo(120).WithMessage("Age must be realistic");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.EmailExistsAsync(email, cancellationToken);
        return !exists;
    }
}
```

## Multiple Validators

You can register multiple validators for the same request. All validators will run, and if any fail, the request will
not reach the handler.

```csharp
public class CreateUserBusinessRulesValidator : IRequestValidator<CreateUserCommand>
{
    public ValueTask<Result> ValidateAsync(
        CreateUserCommand request, 
        CancellationToken cancellationToken = default)
    {
        // Additional business rule validations
        if (request.Email.EndsWith("@tempmail.com"))
            return ValueTask.FromResult(Result.Failure("Temporary email addresses are not allowed"));
        
        return ValueTask.FromResult(Result.Success());
    }
}

// Register both validators
services.AddMediator(config => {
    config.RegisterRequestPipelineBehavior<RequestValidationBehavior<,>>();
    
    config.AddValidator<CreateUserValidator, CreateUserCommand>();
    config.AddValidator<CreateUserBusinessRulesValidator, CreateUserCommand>();
});
```

The `RequestValidationBehavior` will combine all validation errors using the `CombineAsync()` method from
UnambitiousFx.Core.

## Error Handling

When validation fails, the request handler is **not executed**. Instead, the validation behavior returns a
`Result.Failure` with all validation errors.

### Example Handler

```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Result<Guid>> HandleAsync(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        // This code only runs if validation passes
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            Age = request.Age
        };

        await _userRepository.AddAsync(user, cancellationToken);

        return Result.Success(user.Id);
    }
}
```

### Using in a Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.Email, request.Name, request.Age);
        var result = await _sender.SendAsync(command);

        return result.Match(
            userId => CreatedAtAction(nameof(GetUser), new { id = userId }, userId),
            errors => BadRequest(new { Errors = errors.Select(e => e.Message) }));
    }
}
```

### Validation Response Example

If validation fails, you'll get a response like:

```json
{
  "errors": [
    "Email is required",
    "Name must be at least 2 characters",
    "User must be at least 18 years old"
  ]
}
```

## Best Practices

1. **Keep validators focused**: Each validator should validate a specific concern (format, business rules, etc.)
2. **Use async validation judiciously**: Only use async validation when necessary (e.g., database checks)
3. **Register validation behavior early**: Place it before caching or transaction behaviors in the pipeline
4. **Combine validation approaches**: Use basic custom validators for simple checks and FluentValidation for complex
   scenarios
5. **Provide clear error messages**: Make error messages helpful and user-friendly
6. **Consider performance**: Be mindful of expensive validation operations, especially in high-throughput scenarios
7. **Test validators independently**: Write unit tests for your validators separate from handler tests

## Complete Example

Here's a complete example putting it all together:

```csharp
// Request
public record CreateUserCommand(string Email, string Name, int Age) : IRequest<Result<Guid>>;

// Custom validator
public class CreateUserValidator : IRequestValidator<CreateUserCommand>
{
    public ValueTask<Result> ValidateAsync(
        CreateUserCommand request, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return ValueTask.FromResult(Result.Failure("Email is required"));
        
        if (!request.Email.Contains('@'))
            return ValueTask.FromResult(Result.Failure("Invalid email format"));
        
        if (string.IsNullOrWhiteSpace(request.Name))
            return ValueTask.FromResult(Result.Failure("Name is required"));
        
        if (request.Age < 18)
            return ValueTask.FromResult(Result.Failure("Must be 18 or older"));
        
        return ValueTask.FromResult(Result.Success());
    }
}

// Handler
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Result<Guid>> HandleAsync(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            Age = request.Age
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return Result.Success(user.Id);
    }
}

// Registration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator(config =>
        {
            // Register validation behavior
            config.RegisterRequestPipelineBehavior<RequestValidationBehavior<,>>();
            
            // Register validator
            config.AddValidator<CreateUserValidator, CreateUserCommand>();
            
            // Register handler
            config.RegisterRequestHandler<CreateUserCommandHandler, CreateUserCommand, Result<Guid>>();
        });
        
        return services;
    }
}
```

