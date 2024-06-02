using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MicroBlog.Data.EF.SQLServer
{
    [ExcludeFromCodeCoverage]
    public class SQLServerAppDb(DbContextOptions<SQLServerAppDb> options) : AppDb<SQLServerAppDb>(options)
    {
    }
}
