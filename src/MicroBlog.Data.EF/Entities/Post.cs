﻿namespace MicroBlog.Data.EF.Entities
{
    public class Post : IEquatable<Post>
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public required string UserId { get; set; }

        public virtual ICollection<Reaction> Reactions { get; set; } = [];
        public virtual ICollection<Comment> Comments { get; set; } = [];

        public bool Equals(Post? other)
        {
            return Id == other?.Id;
        }
    }
}
