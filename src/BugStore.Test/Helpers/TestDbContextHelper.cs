using BugStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test.Helpers;

public static class TestDbContextHelper
{
    public static AppDbContext CreateInMemoryDbContext(string? dbName = null)
    {
        return new TestAppDbContext(dbName ?? Guid.NewGuid().ToString());
    }

    private class TestAppDbContext(string databaseName) : AppDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName);
        }
    }
}

