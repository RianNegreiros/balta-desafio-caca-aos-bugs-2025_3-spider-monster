using BugStore.Models;

namespace BugStore.Test.Models;

public class ProductTests
{
    [Fact]
    public void Product_Should_Have_Required_Properties()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Test Product",
            Description = "A test product description",
            Slug = "test-product",
            Price = 99.99m
        };

        // Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("Test Product", product.Title);
        Assert.Equal("A test product description", product.Description);
        Assert.Equal("test-product", product.Slug);
        Assert.Equal(99.99m, product.Price);
    }

    [Fact]
    public void Product_Should_Allow_Empty_Guid_Initially()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.Equal(Guid.Empty, product.Id);
    }

    [Fact]
    public void Product_Price_Should_Accept_Decimal_Values()
    {
        // Arrange
        var product = new Product
        {
            // Act
            Price = 19.99m
        };

        Assert.Equal(19.99m, product.Price);

        product.Price = 0.01m;
        Assert.Equal(0.01m, product.Price);

        product.Price = 9999.99m;
        Assert.Equal(9999.99m, product.Price);
    }

    [Fact]
    public void Product_Properties_Should_Be_Settable()
    {
        // Arrange
        var product = new Product();
        var id = Guid.NewGuid();
        var title = "New Product";
        var description = "Product description";
        var slug = "new-product";
        var price = 49.99m;

        // Act
        product.Id = id;
        product.Title = title;
        product.Description = description;
        product.Slug = slug;
        product.Price = price;

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal(title, product.Title);
        Assert.Equal(description, product.Description);
        Assert.Equal(slug, product.Slug);
        Assert.Equal(price, product.Price);
    }
}

