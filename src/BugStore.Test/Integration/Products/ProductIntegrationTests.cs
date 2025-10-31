using BugStore.Models;
using BugStore.Test.Helpers;

namespace BugStore.Test.Integration.Products;

public class ProductIntegrationTests
{
    [Fact]
    public async Task Create_Product_Should_Persist_To_Database()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Integration Test Product",
            Description = "Test Description",
            Slug = "integration-test-product",
            Price = 99.99m
        };

        // Act
        context.Products.Add(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var savedProduct = await context.Products.FindAsync([product.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(savedProduct);
        Assert.Equal(product.Title, savedProduct.Title);
        Assert.Equal(product.Description, savedProduct.Description);
        Assert.Equal(product.Slug, savedProduct.Slug);
        Assert.Equal(product.Price, savedProduct.Price);
    }

    [Fact]
    public async Task Get_Product_By_Id_Should_Return_Correct_Product()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Title = "Test Product",
            Description = "Test Description",
            Slug = "test-product",
            Price = 49.99m
        };

        context.Products.Add(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var retrievedProduct = await context.Products.FindAsync([productId], TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(retrievedProduct);
        Assert.Equal(productId, retrievedProduct.Id);
        Assert.Equal("Test Product", retrievedProduct.Title);
        Assert.Equal(49.99m, retrievedProduct.Price);
    }

    [Fact]
    public async Task Update_Product_Should_Modify_Existing_Product()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Original Title",
            Description = "Original Description",
            Slug = "original-slug",
            Price = 50.00m
        };

        context.Products.Add(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        product.Title = "Updated Title";
        product.Description = "Updated Description";
        product.Slug = "updated-slug";
        product.Price = 75.00m;
        context.Products.Update(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var updatedProduct = await context.Products.FindAsync([product.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Title", updatedProduct.Title);
        Assert.Equal("Updated Description", updatedProduct.Description);
        Assert.Equal("updated-slug", updatedProduct.Slug);
        Assert.Equal(75.00m, updatedProduct.Price);
    }

    [Fact]
    public async Task Delete_Product_Should_Remove_From_Database()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "To Be Deleted",
            Description = "Description",
            Slug = "to-be-deleted",
            Price = 25.00m
        };

        context.Products.Add(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Verify product exists
        var existsBefore = await context.Products.FindAsync([product.Id], TestContext.Current.CancellationToken);
        Assert.NotNull(existsBefore);

        // Act
        context.Products.Remove(product);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var deletedProduct = await context.Products.FindAsync([product.Id], TestContext.Current.CancellationToken);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task List_Products_Should_Return_All_Products()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Title = "Product A", Description = "Desc A", Slug = "product-a", Price = 10.00m },
            new Product { Id = Guid.NewGuid(), Title = "Product B", Description = "Desc B", Slug = "product-b", Price = 20.00m },
            new Product { Id = Guid.NewGuid(), Title = "Product C", Description = "Desc C", Slug = "product-c", Price = 30.00m }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var allProducts = context.Products.ToList();

        // Assert
        Assert.Equal(3, allProducts.Count);
        Assert.All(allProducts, p => Assert.NotNull(p.Title));
        Assert.All(allProducts, p => Assert.True(p.Price > 0));
    }

    [Fact]
    public async Task Product_With_Same_Slug_Should_Be_Allowed()
    {
        // Arrange
        await using var context = TestDbContextHelper.CreateInMemoryDbContext();
        var product1 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product 1",
            Description = "Description 1",
            Slug = "same-slug",
            Price = 10.00m
        };

        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product 2",
            Description = "Description 2",
            Slug = "same-slug",
            Price = 20.00m
        };

        // Act
        context.Products.AddRange(product1, product2);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var productsWithSameSlug = context.Products.Where(p => p.Slug == "same-slug").ToList();
        Assert.Equal(2, productsWithSameSlug.Count);
    }
}

