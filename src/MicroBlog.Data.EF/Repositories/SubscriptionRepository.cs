﻿using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Repositories
{
    public class SubscriptionRepository(IAppDb db, IMapper mapper) : 
        Repository<Subscription, Entities.Subscription>(db.Subscriptions, db, mapper), ISubscriptionRepository
    {
        public async Task DeleteForUser(string userId)
        {
            await DbSet.Where(x => x.FromUserId == userId || x.ToUserId == userId).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}