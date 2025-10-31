using BugStore.Models;

namespace BugStore.Test.Models;

public class CustomerTests
{
    [Fact]
    public void Customer_Should_Have_Required_Properties()
    {
        // Arrange & Act
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "1234567890",
            BirthDate = new DateTime(1990, 1, 1)
        };

        // Assert
        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("john@example.com", customer.Email);
        Assert.Equal("1234567890", customer.Phone);
        Assert.Equal(new DateTime(1990, 1, 1), customer.BirthDate);
    }

    [Fact]
    public void Customer_Should_Allow_Empty_Guid_Initially()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        Assert.Equal(Guid.Empty, customer.Id);
    }

    [Fact]
    public void Customer_Properties_Should_Be_Settable()
    {
        // Arrange
        var customer = new Customer();
        var id = Guid.NewGuid();
        var name = "Jane Smith";
        var email = "jane@example.com";
        var phone = "9876543210";
        var birthDate = new DateTime(1985, 5, 15);

        // Act
        customer.Id = id;
        customer.Name = name;
        customer.Email = email;
        customer.Phone = phone;
        customer.BirthDate = birthDate;

        // Assert
        Assert.Equal(id, customer.Id);
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(phone, customer.Phone);
        Assert.Equal(birthDate, customer.BirthDate);
    }
}

