using NSubstitute;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests;

public sealed class ContextTests {
    private readonly IPublisher _publisher;

    public ContextTests() {
        _publisher = Substitute.For<IPublisher>();
    }

    [Fact]
    public void GivenContextWithPublisher_WhenCreated_ShouldNotBeNull() {
        // Arrange & Act
        var context = new Context(_publisher);

        // Assert
        Assert.NotNull(context);
        Assert.IsType<IContext>(context, false);
    }

    [Fact]
    public void GivenContext_WhenCloned_ShouldCreateNewInstanceWithSameProperties() {
        // Arrange
        var originalContext = new Context(_publisher);
        originalContext.Set("testKey", "testValue");

        // Act
        var clonedContext = originalContext.Clone();

        // Assert
        Assert.NotSame(originalContext, clonedContext);
        if (clonedContext.Get<string>("testKey")
                         .Some(out var value)) {
            Assert.Equal("testValue", value);
        }
        else {
            Assert.Fail("Expected value should be present in cloned context but was not found");
        }
    }

    [Fact]
    public void GivenContext_WhenSetAndGetValue_ShouldStoreAndRetrieveCorrectly() {
        // Arrange
        var context    = new Context(_publisher);
        var testObject = new RequestExample();

        // Act
        context.Set("testKey", testObject);

        // Assert
        if (context.Get<RequestExample>("testKey")
                   .Some(out var retrievedObject)) {
            Assert.Same(testObject, retrievedObject);
        }
        else {
            Assert.Fail("Expected value should be present in context but was not found");
        }
    }

    [Fact]
    public void GivenContext_WhenTryGetNonExistentValue_ShouldReturnFalse() {
        // Arrange
        var context = new Context(_publisher);

        // Act
        var result = context.TryGet<string>("nonExistentKey", out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void GivenContext_WhenTryGetExistingValue_ShouldReturnTrueAndCorrectValue() {
        // Arrange
        var context = new Context(_publisher);
        context.Set("existingKey", 42);

        // Act
        var result = context.TryGet<int>("existingKey", out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
    }
}
