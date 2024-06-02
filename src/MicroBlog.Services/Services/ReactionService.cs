using MicroBlog.Core.Constants;
using MicroBlog.Core.Repositories;
using MicroBlog.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Services.Services
{
    public class ReactionService(IReactionRepository reactionRepository, ICacheService cacheService) : IReactionService
    {
        private readonly IReactionRepository _reactionRepository = reactionRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<IReadOnlyDictionary<ReactionType, int>> GetReactionCountsAsync(Guid postId)
        {
            var reactionCounts = await _cacheService.GetOrCreateAsync(CacheNames.ReactionCounts + postId, async entry =>
            {
                var query = _reactionRepository.GetAll(x => x.PostId == postId, null);
                var reactionTypes = Enum.GetValues<ReactionType>();
                var reactionCounts = new Dictionary<ReactionType, int>();
                foreach (var reactionType in reactionTypes)
                {
                    reactionCounts[reactionType] = await query.Where(x => x.Type == reactionType).CountAsync();
                }

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return reactionCounts.AsReadOnly();
            });

            return reactionCounts!;
        }
    }
}
