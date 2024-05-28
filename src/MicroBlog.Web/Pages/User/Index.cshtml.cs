#nullable disable

using Ganss.Xss;
using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Extensions;
using MicroBlog.Identity.Managers;
using MicroBlog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages.User
{
    [Authorize]
    [BindProperties]
    public class IndexModel(IPostRepository postRepository, ISubscriptionRepository subscriptionRepository, 
        IBlockRepository blockRepository, UserManager userManager, IHtmlSanitizer htmlSanitizer) : PageModel
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IBlockRepository _blockRepository = blockRepository;
        private readonly UserManager _userManager = userManager;
        private readonly IHtmlSanitizer _htmlSanitizer = htmlSanitizer;

        public Identity.Models.User ViewUser { get; set; }
        public bool SameUser { get; set; }
        public bool Subscribed { get; set; }
        public bool Blocked { get; set; }
        public bool BlockedFromUser { get; set; }

        public CreatePostModel CreatePostInput { get; set; }

        public async Task<IActionResult> OnGetAsync(string username)
        {
            ViewUser = await _userManager.FindByNameAsync(username);
            if (ViewUser == null)
            {
                return NotFound($"Unable to load user with username '{username}'.");
            }

            var currentUserId = _userManager.GetUserId(User);
            var currentUserName = _userManager.GetUserName(User);
            SameUser = string.Equals(ViewUser.UserName, currentUserName, StringComparison.InvariantCulture);
            Subscribed = await _subscriptionRepository.Any(x => x.ToUserId == ViewUser.Id && x.FromUserId == currentUserId);
            Blocked = await _blockRepository.Any(x => x.UserId == currentUserId && x.BlockedUserId == ViewUser.Id);
            BlockedFromUser = await _blockRepository.Any(x => x.UserId == ViewUser.Id && x.BlockedUserId == currentUserId);

            return Page();
        }

        public async Task<IActionResult> OnPostCreatePostAsync()
        {
            if (!ModelState.IsValid)
            {
                var username = User.GetUserName()!;
                return RedirectToPage(new { username });
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var sanitizedContent = _htmlSanitizer.Sanitize(CreatePostInput.Content);
                var post = new Core.Models.Post
                {
                    Title = CreatePostInput.Title,
                    Content = sanitizedContent,
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
