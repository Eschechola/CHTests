using CHTests.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHTests.Data.Context
{
    public class TestsContext : DbContext
    {
        public TestsContext(DbContextOptions<TestsContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
    }
}
