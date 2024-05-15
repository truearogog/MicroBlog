#nullable disable

using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages.User
{
    [Authorize]
    [BindProperties]
    public class IndexModel(UserManager userManager, IPostRepository postRepository) : PageModel
    {
        private readonly UserManager _userManager = userManager;
        private readonly IPostRepository _postRepository = postRepository;

        public Identity.Models.User CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound($"Unable to load user with username '{username}'.");
            }

            /*
            await _postRepository.CreateRange(Enumerable.Range(0, 10).Select(x => new Core.Models.Post
            {
                Title = "Title " + x,
                Content = $"<p>{string.Join(' ', Enumerable.Repeat("word", 200))}</p>",
                UserId = user.Id
            }));
            */

            CurrentUser = user;
            return Page();
        }
    }
}
