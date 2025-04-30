---
layout: docs
title: UnambitiousFx.Web
nav_order: 3
---

# UnambitiousFx.Web

UnambitiousFx.Web is a library designed to simplify web development tasks in C# applications. It provides utilities for HTTP requests, API development, HTML manipulation, and more.

## Installation

Install the UnambitiousFx.Web package via NuGet:

```bash
Install-Package UnambitiousFx.Web
```

## Features

### HTTP Client

UnambitiousFx.Web provides a simple, fluent HTTP client:

```csharp
using UnambitiousFx.Web.Http;

// Example HTTP requests
var client = new HttpClientWrapper();

// GET request
var response = await client.GetAsync("https://api.example.com/users");
var users = response.DeserializeJson<List<User>>();

// POST request
var newUser = new User { Name = "John Doe", Email = "john@example.com" };
var createResponse = await client.PostJsonAsync("https://api.example.com/users", newUser);
```

### API Helpers

Simplify API development with helper classes:

```csharp
using UnambitiousFx.Web.Api;

// Example API controller
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public ApiResponse<List<User>> GetUsers()
    {
        try
        {
            var users = _userService.GetAllUsers();
            return ApiResponse<List<User>>.Success(users);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<User>>.Error("Failed to retrieve users", ex);
        }
    }
}
```

### HTML Utilities

Work with HTML content easily:

```csharp
using UnambitiousFx.Web.Html;

// Example HTML operations
var html = "<div><p>Hello, <b>World</b>!</p></div>";
var document = HtmlParser.Parse(html);

// Query elements
var paragraphs = document.QuerySelector("p");
var boldText = document.QuerySelector("b").InnerText; // "World"

// Modify HTML
document.QuerySelector("p").InnerText = "Hello, Universe!";
string modifiedHtml = document.ToString();
```

### URL Manipulation

Manipulate URLs with ease:

```csharp
using UnambitiousFx.Web.Url;

// Example URL operations
var url = new UrlBuilder("https://example.com/search")
    .AddQueryParameter("q", "UnambitiousFx")
    .AddQueryParameter("page", "1")
    .Build(); // https://example.com/search?q=UnambitiousFx&page=1

var components = UrlParser.Parse("https://user:pass@example.com:8080/path?query=value#fragment");
// components.Scheme = "https"
// components.Host = "example.com"
// components.Port = 8080
// etc.
```

### Web Security

Implement common web security practices:

```csharp
using UnambitiousFx.Web.Security;

// Example security operations
// CSRF protection
string token = CsrfProtection.GenerateToken();
bool isValid = CsrfProtection.ValidateToken(receivedToken, token);

// XSS prevention
string sanitized = HtmlSanitizer.Sanitize(userInput);

// Content Security Policy
var csp = new ContentSecurityPolicyBuilder()
    .AllowScriptsFrom("self", "trusted-cdn.com")
    .AllowStylesFrom("self", "styles-cdn.com")
    .Build();
```

## API Reference

For detailed API documentation, please visit the [UnambitiousFx.Web API Reference](/docs/web/api/).

## Examples

Check out our [examples repository](https://github.com/UnambitiousFx/examples) for more examples of using UnambitiousFx.Web.
