using AutoFixture;
using MicroBlog.Core.Models;
using MicroBlog.Data.EF.Repositories;

namespace MicroBlog.Data.EF.Tests.Repositories
{
    public class PostRepositoryTests
    {
        [Fact]
        public void GetAll()
        {
            // arrange
            var mapper = MapperHelpers.GetMapper();
            var fixture = new Fixture();
            var posts = fixture.CreateMany<Entities.Post>(20).OrderBy(x => x.Created);
            var expected = mapper.Map<IEnumerable<Post>>(posts);

            using var dbFixture = new TestAppDbFixture(context =>
            {
                context.Posts.AddRange(posts);
                context.SaveChanges();
            });

            // act
            var repository = new PostRepository(dbFixture.Context, mapper);
            var actual = repository.GetAll();

            // assert
            Assert.Equal(expected.Count(), actual.Count());
            foreach (var (expected_, actual_) in expected.Zip(actual))
            {
                Assert.True(expected_.Equals(actual_));
            }
        }
    }
}