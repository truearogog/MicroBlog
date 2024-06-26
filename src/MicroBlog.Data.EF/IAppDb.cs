﻿using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MicroBlog.Data.EF
{
    public interface IAppDb
    {
        DbSet<Post> Posts { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<Block> Blocks { get; set; }
        DbSet<Reaction> Reactions { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Image> Images { get; set; }

        ChangeTracker ChangeTracker { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
