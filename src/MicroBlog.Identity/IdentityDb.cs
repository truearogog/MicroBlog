using Microsoft.EntityFrameworkCore;
using IdentityDb = Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<MicroBlog.Identity.Models.User>;

namespace MicroBlog.Identity
{
    public abstract class IdentityDb<TDb>(DbContextOptions<TDb> options) : IdentityDb(options) where TDb : IdentityDb
    {
    }
}
