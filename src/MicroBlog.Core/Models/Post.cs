﻿using MicroBlog.Core.Constants;

namespace MicroBlog.Core.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public required string UserId { get; set; }
        public string UserProfilePictureUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public IReadOnlyDictionary<ReactionType, int> ReactionCounts { get; set; } = new Dictionary<ReactionType, int>();
    }
}
