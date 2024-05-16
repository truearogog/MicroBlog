#nullable disable

using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Managers;
using MicroBlog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages.User
{
    [Authorize]
    [BindProperties]
    public class IndexModel(IPostRepository postRepository, UserManager userManager) : PageModel
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly UserManager _userManager = userManager;

        public bool SameUser { get; set; }
        public Identity.Models.User ViewUser { get; set; }

        public CreatePostModel CreatePostInput { get; set; }

        public async Task<IActionResult> OnGetAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound($"Unable to load user with username '{username}'.");
            }

            SameUser = string.Equals(user.UserName, username, StringComparison.InvariantCulture);
            ViewUser = user;

            return Page();
        }

        public async Task<IActionResult> OnPostCreatePostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var post = new Core.Models.Post
                {
                    Title = CreatePostInput.Title,
                    Content = CreatePostInput.Content,
                    UserId = user!.Id
                };

                await _postRepository.Create(post);
                return RedirectToPage(new { user.UserName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
