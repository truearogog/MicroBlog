using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MicroBlog.Identity.SQLServer
{
    [ExcludeFromCodeCoverage]
    public class SQLServerIdentityDb(DbContextOptions<SQLServerIdentityDb> options) : IdentityDb<SQLServerIdentityDb>(options)
    {
    }
}
