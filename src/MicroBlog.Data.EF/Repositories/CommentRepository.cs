using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroBlog.Data.EF.Repositories
{
    public class CommentRepository(IAppDb db, IMapper mapper) :
        Repository<Comment, Entities.Comment>(db.Comments, db, mapper), ICommentRepository
    {
        public override IQueryable<Comment> GetAll(Expression<Func<Comment, bool>>? filter = null, Func<IQueryable<Comment>, IOrderedQueryable<Comment>>? orderBy = null)
        {
            orderBy ??= x => x.OrderByDescending(y => y.Created);
            return base.GetAll(filter, orderBy);
        }

        public async Task DeleteForUser(string userId)
        {
            await DbSet.Where(x => x.UserId == userId).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
