﻿using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroBlog.Data.EF.Repositories
{
    public class PostRepository(IAppDb db, IMapper mapper) : 
        Repository<Post, Entities.Post>(db.Posts, db, mapper), IPostRepository
    {
        public override IQueryable<Post> GetAll(Expression<Func<Post, bool>>? filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>>? orderBy = null)
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
