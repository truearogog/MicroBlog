using MicroBlog.Core.Constants;

namespace MicroBlog.Core.Services
{
    public interface IReactionService
    {
        Task<IReadOnlyDictionary<ReactionType, int>> GetReactionCountsAsync(Guid postId);
    }
}
