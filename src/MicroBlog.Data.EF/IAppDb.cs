using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF
{
    public interface IAppDb
    {
        DbSet<Post> Posts { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
