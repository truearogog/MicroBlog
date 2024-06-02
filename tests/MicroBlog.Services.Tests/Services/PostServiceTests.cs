using MicroBlog.Core.Constants;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using MicroBlog.Core.Services;
using MicroBlog.Identity.Managers;
using MicroBlog.Services.Services;
using Moq;
using System.Linq.Expressions;

namespace MicroBlog.Services.Tests.Services
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepository;
        private readonly Mock<IUserManager> _mockUserManager;
        private readonly Mock<IReactionService> _mockReactionService;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _mockSubscriptionRepository = new Mock<ISubscriptionRepository>();
            _mockUserManager = new Mock<IUserManager>();
            _mockReactionService = new Mock<IReactionService>();
            _postService = new PostService(_mockPostRepository.Object, _mockSubscriptionRepository.Object, _mockUserManager.Object, _mockReactionService.Object);
        }

        [Fact]
        public async Task GetPostsFromUserAsync_ReturnsPosts()
        {
            // Arrange
            var userId = "user1";
            var posts = new List<Post>
            {
                new() { Id = Guid.NewGuid(), UserId = userId, Title="Title 1", Content="Content 1", Created = DateTime.UtcNow.AddDays(-1) },
                new() { Id = Guid.NewGuid(), UserId = userId, Title="Title 2", Content="Content 2", Created = DateTime.UtcNow.AddDays(-2) },
            }.AsQueryable();
            _mockPostRepository.Setup(r => r.GetAll(It.IsAny<Expression<Func<Post, bool>>>(), null))
                .Returns(posts);

            _mockUserManager.Setup(m => m.GetProfilePictureUrlAsync(It.IsAny<string>()))
                .ReturnsAsync("url");
            _mockUserManager.Setup(m => m.GetUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync("username");
            _mockReactionService.Setup(r => r.GetReactionCountsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Dictionary<ReactionType, int>());

            // Act
            var result = (await _postService.GetPostsFromUserAsync(userId, DateTime.UtcNow, 0, 10)).ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPostsForUserAsync_ReturnsPosts()
        {
            // Arrange
            var userId = "user1";
            var subscriptions = new List<Subscription>
            {
                new() { FromUserId = userId, ToUserId = "user2" },
                new() { FromUserId = userId, ToUserId = "user3" }
            }.AsQueryable();

            var posts = new List<Post>
            {
                new() { Id = Guid.NewGuid(), UserId = "user2", Title="Title 1", Content="Content 1", Created = DateTime.UtcNow.AddDays(-1) },
                new() { Id = Guid.NewGuid(), UserId = "user3", Title="Title 2", Content="Content 2", Created = DateTime.UtcNow.AddDays(-2) },
            }.AsQueryable();

            _mockSubscriptionRepository.Setup(r => r.GetAll(It.IsAny<Expression<Func<Subscription, bool>>>(), null))
                .Returns(subscriptions);
            _mockPostRepository.Setup(r => r.GetAll(It.IsAny<Expression<Func<Post, bool>>>(), null))
                .Returns(posts);

            _mockUserManager.Setup(m => m.GetProfilePictureUrlAsync(It.IsAny<string>()))
                .ReturnsAsync("url");
            _mockUserManager.Setup(m => m.GetUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync("username");
            _mockReactionService.Setup(r => r.GetReactionCountsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Dictionary<ReactionType, int>());

            // Act
            var result = (await _postService.GetPostsForUserAsync(userId, DateTime.UtcNow, 0, 10)).ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
