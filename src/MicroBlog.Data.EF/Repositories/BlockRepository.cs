using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Repositories
{
    public class BlockRepository(IAppDb db, IMapper mapper) :
        Repository<Block, Entities.Block>(db.Blocks, db, mapper), IBlockRepository
    {
        public override async Task DeleteRange(IEnumerable<Block> models)
        {
            await DbSet.Where(x => models.Any(y => x.UserId == y.UserId && x.BlockedUserId == y.BlockedUserId))
                .ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteForUser(string userId)
        {
            await DbSet.Where(x => x.UserId == userId || x.BlockedUserId == userId).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
