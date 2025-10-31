using BugStore.Data;
using BugStore.Models;
using BugStore.Test.Helpers;

namespace BugStore.Test.Integration.Customers;

public class CustomerIntegrationTests
{
    [Fact]
    public async Task Create_Customer_Should_Persist_To_Database()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test Customer",
            Email = "integration@test.com",
            Phone = "5551234567",
            BirthDate = new DateTime(1990, 5, 15)
        };

        // Act
        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedCustomer = await context.Customers.FindAsync([customer.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedCustomer);
        Assert.Equal(customer.Name, savedCustomer.Name);
        Assert.Equal(customer.Email, savedCustomer.Email);
        Assert.Equal(customer.Phone, savedCustomer.Phone);
        Assert.Equal(customer.BirthDate, savedCustomer.BirthDate);
    }

    [Fact]
    public async Task Get_Customer_By_Id_Should_Return_Correct_Customer()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            Name = "Test Customer",
            Email = "test@example.com",
            Phone = "1234567890",
            BirthDate = DateTime.Now.AddYears(-25)
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var retrievedCustomer = await context.Customers.FindAsync([customerId], TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(retrievedCustomer);
        Assert.Equal(customerId, retrievedCustomer.Id);
        Assert.Equal("Test Customer", retrievedCustomer.Name);
    }

    [Fact]
    public async Task Update_Customer_Should_Modify_Existing_Customer()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Email = "original@test.com",
            Phone = "1111111111",
            BirthDate = DateTime.Now.AddYears(-30)
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        customer.Name = "Updated Name";
        customer.Email = "updated@test.com";
        customer.Phone = "2222222222";
        context.Customers.Update(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var updatedCustomer = await context.Customers.FindAsync([customer.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(updatedCustomer);
        Assert.Equal("Updated Name", updatedCustomer.Name);
        Assert.Equal("updated@test.com", updatedCustomer.Email);
        Assert.Equal("2222222222", updatedCustomer.Phone);
    }

    [Fact]
    public async Task Delete_Customer_Should_Remove_From_Database()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "To Be Deleted",
            Email = "delete@test.com",
            Phone = "9999999999",
            BirthDate = DateTime.Now.AddYears(-20)
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Verify customer exists
        var existsBefore = await context.Customers.FindAsync(new object?[] { customer.Id }, TestContext.Current.CancellationToken);
        Assert.NotNull(existsBefore);

        // Act
        context.Customers.Remove(customer);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var deletedCustomer = await context.Customers.FindAsync(new object?[] { customer.Id }, TestContext.Current.CancellationToken);
        Assert.Null(deletedCustomer);
    }

    [Fact]
    public async Task List_Customers_Should_Return_All_Customers()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var customers = new List<Customer>
        {
            new() { Id = Guid.NewGuid(), Name = "Customer A", Email = "a@test.com", Phone = "111", BirthDate = DateTime.Now.AddYears(-25) },
            new() { Id = Guid.NewGuid(), Name = "Customer B", Email = "b@test.com", Phone = "222", BirthDate = DateTime.Now.AddYears(-30) },
            new() { Id = Guid.NewGuid(), Name = "Customer C", Email = "c@test.com", Phone = "333", BirthDate = DateTime.Now.AddYears(-35) }
        };

        context.Customers.AddRange(customers);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var allCustomers = context.Customers.ToList();

        // Assert
        Assert.Equal(3, allCustomers.Count);
        Assert.All(allCustomers, c => Assert.NotNull(c.Name));
        Assert.All(allCustomers, c => Assert.NotNull(c.Email));
    }
}

