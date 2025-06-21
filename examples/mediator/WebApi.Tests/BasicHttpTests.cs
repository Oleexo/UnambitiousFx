using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests;

public sealed class BasicHttpTests
    : IClassFixture<WebApplicationFactory<Program>> {
    private readonly WebApplicationFactory<Program> _factory;

    public BasicHttpTests(WebApplicationFactory<Program> factory) {
        _factory = factory;
    }

    private async Task<Guid> CreateTodoOnServer() {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/todos", new {
            Name = "Test"
        }, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
        var location = response.Headers.Location;
        Assert.NotNull(location);
        var id = Guid.Parse(location.ToString()
                                    .Split('/')
                                    .Last());
        return id;
    }

    [Fact]
    public async Task CreateTodo_ReturnsSuccess() {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/todos", new {
            Name = "Test"
        }, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task GetTodos_ReturnsSuccess() {
        // Arrange
        await CreateTodoOnServer();
        await CreateTodoOnServer();
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/todos", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task GetTodo_ReturnsSuccess() {
        // Arrange
        var id     = await CreateTodoOnServer();
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/todos/{id}", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task UpdateTodo_ReturnsSuccess() {
        // Arrange
        var id     = await CreateTodoOnServer();
        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync($"/todos/{id}", new {
            Name = "Updated"
        }, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task DeleteTodo_ReturnSuccess() {
        // Arrange
        var id     = await CreateTodoOnServer();
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/todos/{id}", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}
