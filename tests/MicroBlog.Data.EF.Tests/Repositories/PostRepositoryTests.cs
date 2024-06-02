using AutoFixture;
using AutoFixture.Dsl;
using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Data.EF.Profiles;
using MicroBlog.Data.EF.Repositories;
using MicroBlog.Data.EF.Tests.AutoFixture;

namespace MicroBlog.Data.EF.Tests.Repositories
{
    public class PostRepositoryTests : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly IPostprocessComposer<Entities.Post> _fixture;

        public PostRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(PostProfile));
            });
            _mapper = new Mapper(config);
            _fixture = new Fixture().Customize(new IgnoreVirtualMembersCustomisation()).Build<Entities.Post>()
                .Without(x => x.Id)
                .Without(x => x.Created)
                .Without(x => x.Updated);
        }

        public void Dispose()
        {
        }

        [Fact]
        public void GetAll()
        {
            // arrange
            var posts = _fixture.CreateMany(20);
            using var appDb = new TestAppDb(new());
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();
            var expected = posts.Select(x => x.Id).ToList();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var actual = repository.GetAll();

            // assert
            Assert.Equal(expected.Count, actual.Count());
            foreach (var actual_ in actual)
            {
                Assert.Contains(actual_.Id, expected);
            }
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Find()
        {
            // arrange
            var posts = _fixture.CreateMany(20);
            using var appDb = new TestAppDb(new());
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();
            var expected = posts.First().Id;

            // act
            var repository = new PostRepository(appDb, _mapper);
            var actual = await repository.Find(expected);

            // assert
            Assert.Equal(expected, actual.Id);
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Any_Ok()
        {
            // arrange
            var posts = _fixture.CreateMany(20);
            using var appDb = new TestAppDb(new());
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();
            var expected = posts.First().Id;

            // act
            var repository = new PostRepository(appDb, _mapper);
            var actual = await repository.Any(x => x.Id == expected);

            // assert
            Assert.True(actual);
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Any_Fail()
        {
            // arrange
            var posts = _fixture.CreateMany(20);
            using var appDb = new TestAppDb(new());
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();
            var expected = Guid.NewGuid();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var actual = await repository.Any(x => x.Id == expected);

            // assert
            Assert.False(actual);
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task All()
        {
            // arrange
            var posts = _fixture.CreateMany(20);
            using var appDb = new TestAppDb(new());
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var actual = await repository.All(x => x.Created < DateTime.UtcNow);

            // assert
            Assert.True(actual);
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Create()
        {
            // arrange
            using var appDb = new TestAppDb(new());

            // act
            var repository = new PostRepository(appDb, _mapper);
            var post = _mapper.Map<Post>(_fixture.Create());
            var expected = await repository.Create(post);

            // assert
            var actual = appDb.Posts.Where(x => x.Id == expected.Id).FirstOrDefault();
            Assert.NotNull(actual);
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task CreateRange()
        {
            // arrange
            using var appDb = new TestAppDb(new());

            // act
            var repository = new PostRepository(appDb, _mapper);
            var posts = _mapper.Map<IEnumerable<Post>>(_fixture.CreateMany(20));
            var expected = await repository.CreateRange(posts);

            // assert
            foreach (var e in expected)
            {
                Assert.NotNull(appDb.Posts.FirstOrDefault(x => x.Id == e.Id));
            }
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Update()
        {
            // arrange
            using var appDb = new TestAppDb(new());
            var post = _fixture.Create();
            appDb.Posts.Add(post);
            appDb.SaveChanges();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var updatedPost = new Post
            {
                Id = post.Id,
                Title = "Updated",
                Content = post.Content,
                UserId = post.UserId,
            };
            await repository.Update(updatedPost);

            // assert
            Assert.Equal("Updated", appDb.Posts.First(x => x.Id == post.Id).Title);
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task UpdateRange()
        {
            // arrange
            using var appDb = new TestAppDb(new());
            var posts = _fixture.CreateMany(20);
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var updatedPosts = posts.Select(post => new Post
            {
                Id = post.Id,
                Title = "Updated",
                Content = post.Content,
                UserId = post.UserId,
            });
            await repository.UpdateRange(updatedPosts);

            // assert
            foreach (var post in posts)
            {
                Assert.True(appDb.Posts.FirstOrDefault(x => x.Id == post.Id)?.Title == "Updated");
            }
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Delete_Model()
        {
            // arrange
            using var appDb = new TestAppDb(new());
            var post = _fixture.Create();
            appDb.Posts.Add(post);
            appDb.SaveChanges();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var model = _mapper.Map<Post>(post);
            await repository.Delete(model);

            // assert
            Assert.Null(appDb.Posts.FirstOrDefault(x => x.Id == post.Id));
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Delete_Keys()
        {
            // arrange
            using var appDb = new TestAppDb(new());
            var post = _fixture.Create();
            appDb.Posts.Add(post);
            appDb.SaveChanges();

            // act
            var repository = new PostRepository(appDb, _mapper);
            await repository.Delete(post.Id);

            // assert
            Assert.Null(appDb.Posts.FirstOrDefault(x => x.Id == post.Id));
            appDb.Database.EnsureDeleted();
        }

        [Fact]
        public async Task DeleteRange()
        {
            // arrange
            using var appDb = new TestAppDb(new());
            var posts = _fixture.CreateMany(20);
            appDb.Posts.AddRange(posts);
            appDb.SaveChanges();

            // act
            var repository = new PostRepository(appDb, _mapper);
            var models = _mapper.Map<IEnumerable<Post>>(posts);
            await repository.DeleteRange(models);

            // assert
            foreach (var post in posts)
            {
                Assert.Null(appDb.Posts.FirstOrDefault(x => x.Id == post.Id));
            }
            appDb.Database.EnsureDeleted();
        }
    }
}