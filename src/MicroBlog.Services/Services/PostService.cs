﻿using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using MicroBlog.Core.Services;
using MicroBlog.Identity.Managers;
using System.Linq.Expressions;

namespace MicroBlog.Services.Services
{
    public class PostService(IPostRepository postRepository, ISubscriptionRepository subscriptionRepository, 
        UserManager userManager, IReactionService reactionService) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly UserManager _userManager = userManager;
        private readonly IReactionService _reactionService = reactionService;

        private async Task<IEnumerable<Post>> GetPostsAsync(Expression<Func<Post, bool>> filter, int skip, int take)
        {
            var posts = _postRepository.GetAll(filter).Skip(skip).Take(take).ToList();
            foreach (var post in posts)
            {
                post.UserProfilePictureUrl = await _userManager.GetProfilePictureUrlAsync(post.UserId);
                post.UserName = await _userManager.GetUserNameAsync(post.UserId);
                post.ReactionCounts = await _reactionService.GetReactionCountsAsync(post.Id);
            }
            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromUserAsync(string userId, int skip, int take)
        {
            return await GetPostsAsync(x => x.UserId == userId, skip, take);
        }

        public async Task<IEnumerable<Post>> GetPostsForUserAsync(string userId, int skip, int take)
        {
            var subscriptions = _subscriptionRepository.GetAll(x => x.FromUserId == userId).Select(x => x.ToUserId).ToHashSet();
            return await GetPostsAsync(x => subscriptions.Contains(x.UserId), skip, take);
        }
    }
}
