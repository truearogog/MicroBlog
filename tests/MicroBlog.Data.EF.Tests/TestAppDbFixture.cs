using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MicroBlog.Data.EF.Tests
{
    internal class TestAppDbFixture : IDisposable
    {
        public readonly TestAppDb Context;
        private readonly DbConnection _connection;

        public TestAppDbFixture(Action<TestAppDb> setupAction)
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<TestAppDb>()
                .UseSqlite(_connection)
                .Options;

            Context = new TestAppDb(options);
            Context.Database.EnsureCreated();
            setupAction(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
