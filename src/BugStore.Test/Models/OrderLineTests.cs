using BugStore.Models;

namespace BugStore.Test.Models;

public class OrderLineTests
{
    [Fact]
    public void OrderLine_Should_Have_Required_Properties()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        // Act
        var orderLine = new OrderLine
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            Quantity = 5,
            Total = 250.00m
        };

        // Assert
        Assert.NotEqual(Guid.Empty, orderLine.Id);
        Assert.Equal(orderId, orderLine.OrderId);
        Assert.Equal(productId, orderLine.ProductId);
        Assert.Equal(5, orderLine.Quantity);
        Assert.Equal(250.00m, orderLine.Total);
    }

    [Fact]
    public void OrderLine_Quantity_Should_Accept_Zero()
    {
        // Arrange
        var orderLine = new OrderLine
        {
            // Act
            Quantity = 0
        };

        // Assert
        Assert.Equal(0, orderLine.Quantity);
    }

    [Fact]
    public void OrderLine_Quantity_Should_Accept_Negative_Values()
    {
        // Arrange
        var orderLine = new OrderLine
        {
            // Act
            Quantity = -1
        };

        // Assert
        Assert.Equal(-1, orderLine.Quantity);
    }

    [Fact]
    public void OrderLine_Total_Should_Accept_Decimal_Values()
    {
        // Arrange
        var orderLine = new OrderLine
        {
            // Act & Assert
            Total = 0.01m
        };

        Assert.Equal(0.01m, orderLine.Total);

        orderLine.Total = 99999.99m;
        Assert.Equal(99999.99m, orderLine.Total);

        orderLine.Total = 0m;
        Assert.Equal(0m, orderLine.Total);
    }

    [Fact]
    public void OrderLine_Should_Allow_Product_Navigation_Property()
    {
        // Arrange
        var orderLine = new OrderLine();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Test Product",
            Description = "Description",
            Slug = "test-product",
            Price = 50.00m
        };

        // Act
        orderLine.ProductId = product.Id;
        orderLine.Product = product;

        // Assert
        Assert.NotNull(orderLine.Product);
        Assert.Equal(product.Id, orderLine.ProductId);
        Assert.Equal("Test Product", orderLine.Product.Title);
    }
}

