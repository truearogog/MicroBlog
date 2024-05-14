using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Tests
{
    internal sealed class TestAppDb(DbContextOptions<TestAppDb> options) : AppDb<TestAppDb>(options)
    {
    }
}
