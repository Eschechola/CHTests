using CHTests.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CHTests.UnitTests.Fakes
{
    public class FakeContext : TestsContext
    {
        public FakeContext(DbContextOptions<TestsContext> options) : base(options)
        { }
    }
}
