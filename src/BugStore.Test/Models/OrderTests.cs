using BugStore.Models;

namespace BugStore.Test.Models;

public class OrderTests
{
    [Fact]
    public void Order_Should_Have_Required_Properties()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;

        // Act
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Assert
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(createdAt, order.CreatedAt);
        Assert.Equal(updatedAt, order.UpdatedAt);
    }

    [Fact]
    public void Order_Should_Initialize_Lines_As_Null()
    {
        // Arrange & Act
        var order = new Order();

        // Assert
        Assert.Null(order.Lines);
    }

    [Fact]
    public void Order_Should_Allow_Lines_To_Be_Assigned()
    {
        // Arrange
        var order = new Order();
        var lines = new List<OrderLine>
        {
            new() { Id = Guid.NewGuid(), Quantity = 1, Total = 10.00m },
            new() { Id = Guid.NewGuid(), Quantity = 2, Total = 20.00m }
        };

        // Act
        order.Lines = lines;

        // Assert
        Assert.NotNull(order.Lines);
        Assert.Equal(2, order.Lines.Count);
    }

    [Fact]
    public void Order_Should_Allow_Customer_Navigation_Property()
    {
        // Arrange
        var order = new Order();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            Email = "test@example.com",
            Phone = "1234567890",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        // Act
        order.CustomerId = customer.Id;
        order.Customer = customer;

        // Assert
        Assert.NotNull(order.Customer);
        Assert.Equal(customer.Id, order.CustomerId);
        Assert.Equal("Test Customer", order.Customer.Name);
    }
}

