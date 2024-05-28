using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Repositories
{
    public class ImageRepository(IAppDb db, IMapper mapper) :
        Repository<Image, Entities.Image>(db.Images, db, mapper), IImageRepository
    {
        public override async Task DeleteRange(IEnumerable<Image> models)
        {
            await DbSet.Where(x => models.Any(y => x.Path == y.Path)).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteForUser(string userId)
        {
            await DbSet.Where(x => x.UserId == userId).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
