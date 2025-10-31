using BugStore.Data;
using BugStore.Models;
using BugStore.Test.Helpers;

namespace BugStore.Test.Integration.Orders;

public class OrderIntegrationTests
{
    [Fact]
    public async Task Create_Order_With_Customer_Should_Persist_To_Database()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Order Customer",
            Email = "order@test.com",
            Phone = "5551234567",
            BirthDate = DateTime.Now.AddYears(-28)
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        context.Customers.Add(customer);
        context.Orders.Add(order);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedOrder = await context.Orders.FindAsync([order.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedOrder);
        Assert.Equal(order.CustomerId, savedOrder.CustomerId);
        Assert.Equal(customer.Id, savedOrder.CustomerId);
    }

    [Fact]
    public async Task Create_Order_With_OrderLines_Should_Persist_All_Data()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Customer",
            Email = "customer@test.com",
            Phone = "111",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        var product1 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product 1",
            Description = "Description 1",
            Slug = "product-1",
            Price = 50.00m
        };

        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product 2",
            Description = "Description 2",
            Slug = "product-2",
            Price = 75.00m
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var orderLine1 = new OrderLine
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            ProductId = product1.Id,
            Quantity = 2,
            Total = 100.00m
        };

        var orderLine2 = new OrderLine
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            ProductId = product2.Id,
            Quantity = 1,
            Total = 75.00m
        };

        // Act
        context.Customers.Add(customer);
        context.Products.AddRange(product1, product2);
        context.Orders.Add(order);
        context.OrderLines.AddRange(orderLine1, orderLine2);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedOrder = await context.Orders.FindAsync([order.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedOrder);

        var savedOrderLines = context.OrderLines.Where(ol => ol.OrderId == order.Id).ToList();
        Assert.Equal(2, savedOrderLines.Count);
        Assert.Equal(2, savedOrderLines.First(ol => ol.ProductId == product1.Id).Quantity);
        Assert.Equal(1, savedOrderLines.First(ol => ol.ProductId == product2.Id).Quantity);
    }

    [Fact]
    public async Task Update_Order_Should_Update_Timestamp()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Customer",
            Email = "customer@test.com",
            Phone = "111",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        var originalTime = DateTime.UtcNow.AddMinutes(-10);
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            CreatedAt = originalTime,
            UpdatedAt = originalTime
        };

        context.Customers.Add(customer);
        context.Orders.Add(order);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        await Task.Delay(100, TestContext.Current.CancellationToken);
        order.UpdatedAt = DateTime.UtcNow;
        context.Orders.Update(order);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var updatedOrder = await context.Orders.FindAsync([order.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(updatedOrder);
        Assert.True(updatedOrder.UpdatedAt > updatedOrder.CreatedAt);
    }

    [Fact]
    public async Task Delete_Order_Should_Also_Delete_OrderLines()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Customer",
            Email = "customer@test.com",
            Phone = "111",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product",
            Description = "Description",
            Slug = "product",
            Price = 50.00m
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var orderLine = new OrderLine
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            ProductId = product.Id,
            Quantity = 1,
            Total = 50.00m
        };

        context.Customers.Add(customer);
        context.Products.Add(product);
        context.Orders.Add(order);
        context.OrderLines.Add(orderLine);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        context.OrderLines.Remove(orderLine);
        context.Orders.Remove(order);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var deletedOrder = await context.Orders.FindAsync([order.Id], TestContext.Current.CancellationToken);
        Assert.Null(deletedOrder);

        var deletedOrderLine = await context.OrderLines.FindAsync([orderLine.Id], TestContext.Current.CancellationToken);
        Assert.Null(deletedOrderLine);
    }
}

