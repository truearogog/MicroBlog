using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Managers;
using MicroBlog.Services.Services;
using Moq;
using System.Linq.Expressions;

namespace MicroBlog.Services.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IUserManager> _mockUserManager;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockUserManager = new Mock<IUserManager>();
            _commentService = new CommentService(_mockCommentRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task GetComments_ReturnsComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new() { UserId = "user1", Content = "content1", Id = Guid.NewGuid(), PostId = postId, Created = DateTime.UtcNow.AddDays(-1) },
                new() { UserId = "user2", Content = "content2", Id = Guid.NewGuid(), PostId = postId, Created = DateTime.UtcNow.AddDays(-2) }
            }.AsQueryable();

            _mockCommentRepository.Setup(r => r.GetAll(It.IsAny<Expression<Func<Comment, bool>>>(), null))
                                  .Returns(comments);

            _mockUserManager.Setup(m => m.GetProfilePictureUrlAsync(It.IsAny<string>()))
                            .ReturnsAsync("url");
            _mockUserManager.Setup(m => m.GetUserNameAsync(It.IsAny<string>()))
                            .ReturnsAsync("username");

            // Act
            var result = (await _commentService.GetComments(postId, DateTime.UtcNow, 0, 10)).ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CreateComment_CreatesComment()
        {
            // Arrange
            var comment = new Comment { UserId = "user1", Content = "content1", Id = Guid.NewGuid(), PostId = Guid.NewGuid(), Created = DateTime.UtcNow };
            var entity = new Comment { UserId = "user1", Content = "content1", Id = comment.Id, PostId = comment.PostId, Created = comment.Created };

            _mockCommentRepository.Setup(r => r.Create(It.IsAny<Comment>())).ReturnsAsync(entity);
            _mockUserManager.Setup(m => m.GetProfilePictureUrlAsync(It.IsAny<string>()))
                            .ReturnsAsync("url");
            _mockUserManager.Setup(m => m.GetUserNameAsync(It.IsAny<string>()))
                            .ReturnsAsync("username");

            // Act
            var result = await _commentService.CreateComment(comment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(comment.Id, result.Id);
        }
    }
}
