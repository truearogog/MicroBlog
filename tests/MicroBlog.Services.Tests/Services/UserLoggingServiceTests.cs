using MicroBlog.Services.Services;

namespace MicroBlog.Services.Tests.Services
{
    public class UserLoggingServiceTests
    {
        private readonly UserLoggingService _userLoggingService;

        public UserLoggingServiceTests()
        {
            _userLoggingService = new UserLoggingService();
        }

        [Fact]
        public void AddOrUpdateLogin_AddsOrUpdatesLogin()
        {
            // Arrange
            var userId = "user1";
            var dateTime = DateTime.UtcNow;

            // Act
            _userLoggingService.AddOrUpdateLogin(userId, dateTime);
            var result = _userLoggingService.GetLogins();

            // Assert
            Assert.Single(result);
            Assert.Equal(dateTime, result[userId]);
        }

        [Fact]
        public void ResetLogins_ResetsLogins()
        {
            // Arrange
            var userId = "user1";
            var dateTime = DateTime.UtcNow;
            _userLoggingService.AddOrUpdateLogin(userId, dateTime);

            // Act
            _userLoggingService.ResetLogins();
            var result = _userLoggingService.GetLogins();

            // Assert
            Assert.Empty(result);
        }
    }
}
