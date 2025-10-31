using BugStore.Models;
using BugStore.Test.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Data;

public class AppDbContextTests
{
    [Fact]
    public void AppDbContext_Should_Have_Customers_DbSet()
    {
        // Arrange
        using var context = TestDbContextHelper.CreateInMemoryDbContext();

        // Act & Assert
        Assert.NotNull(context.Customers);
    }

    [Fact]
    public void AppDbContext_Should_Have_Products_DbSet()
    {
        // Arrange
        using var context = TestDbContextHelper.CreateInMemoryDbContext();

        // Act & Assert
        Assert.NotNull(context.Products);
    }

    [Fact]
    public void AppDbContext_Should_Have_Orders_DbSet()
    {
        // Arrange
        using var context = TestDbContextHelper.CreateInMemoryDbContext();

        // Act & Assert
        Assert.NotNull(context.Orders);
    }

    [Fact]
    public void AppDbContext_Should_Have_OrderLines_DbSet()
    {
        // Arrange
        using var context = TestDbContextHelper.CreateInMemoryDbContext();

        // Act & Assert
        Assert.NotNull(context.OrderLines);
    }

    [Fact]
    public async Task AppDbContext_Should_Save_Customer()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            Email = "test@example.com",
            Phone = "1234567890",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        // Act
        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedCustomer = await context.Customers.FindAsync([customer.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedCustomer);
        Assert.Equal(customer.Name, savedCustomer.Name);
        Assert.Equal(customer.Email, savedCustomer.Email);
    }

    [Fact]
    public async Task AppDbContext_Should_Save_Product()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Test Product",
            Description = "Test Description",
            Slug = "test-product",
            Price = 99.99m
        };

        // Act
        context.Products.Add(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedProduct = await context.Products.FindAsync([product.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedProduct);
        Assert.Equal(product.Title, savedProduct.Title);
        Assert.Equal(product.Price, savedProduct.Price);
    }

    [Fact]
    public async Task AppDbContext_Should_Save_Order_With_Customer()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            Email = "test@example.com",
            Phone = "1234567890",
            BirthDate = DateTime.Now.AddYears(-30)
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
    }

    [Fact]
    public async Task AppDbContext_Should_Save_Order_With_OrderLines()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            Email = "test@example.com",
            Phone = "1234567890",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Test Product",
            Description = "Test Description",
            Slug = "test-product",
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
            Quantity = 2,
            Total = 100.00m
        };

        // Act
        context.Customers.Add(customer);
        context.Products.Add(product);
        context.Orders.Add(order);
        context.OrderLines.Add(orderLine);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedOrderLine = await context.OrderLines.FindAsync([orderLine.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedOrderLine);
        Assert.Equal(orderLine.OrderId, savedOrderLine.OrderId);
        Assert.Equal(orderLine.ProductId, savedOrderLine.ProductId);
        Assert.Equal(orderLine.Quantity, savedOrderLine.Quantity);
    }

    [Fact]
    public async Task AppDbContext_Should_Query_Customers()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customers = new List<Customer>
        {
            new() { Id = Guid.NewGuid(), Name = "Customer 1", Email = "c1@test.com", Phone = "111", BirthDate = DateTime.Now.AddYears(-25) },
            new() { Id = Guid.NewGuid(), Name = "Customer 2", Email = "c2@test.com", Phone = "222", BirthDate = DateTime.Now.AddYears(-30) },
            new() { Id = Guid.NewGuid(), Name = "Customer 3", Email = "c3@test.com", Phone = "333", BirthDate = DateTime.Now.AddYears(-35) }
        };

        context.Customers.AddRange(customers);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var allCustomers = await context.Customers.ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(3, allCustomers.Count);
    }

    [Fact]
    public async Task AppDbContext_Should_Update_Customer()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Email = "original@test.com",
            Phone = "111",
            BirthDate = DateTime.Now.AddYears(-25)
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        customer.Name = "Updated Name";
        customer.Email = "updated@test.com";
        context.Customers.Update(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var updatedCustomer = await context.Customers.FindAsync([customer.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(updatedCustomer);
        Assert.Equal("Updated Name", updatedCustomer.Name);
        Assert.Equal("updated@test.com", updatedCustomer.Email);
    }

    [Fact]
    public async Task AppDbContext_Should_Delete_Customer()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            Email = "delete@test.com",
            Phone = "111",
            BirthDate = DateTime.Now.AddYears(-25)
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        context.Customers.Remove(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var deletedCustomer = await context.Customers.FindAsync([customer.Id], TestContext.Current.CancellationToken);
        Assert.Null(deletedCustomer);
    }
}

