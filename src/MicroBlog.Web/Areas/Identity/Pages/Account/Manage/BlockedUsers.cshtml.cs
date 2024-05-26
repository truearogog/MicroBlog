using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Extensions;
using MicroBlog.Identity.Managers;
using MicroBlog.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Areas.Identity.Pages.Account.Manage
{
    [BindProperties]
    public class BlockedUsersModel(IBlockRepository blockRepository, UserManager userManager) : PageModel
    {
        private readonly IBlockRepository _blockRepository = blockRepository;
        private readonly UserManager _userManager = userManager;

        public IEnumerable<User> BlockedUsers { get; set; } = Enumerable.Empty<User>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.GetUserId()!;

            var blockedUsers = _blockRepository.GetAll(x => x.UserId == userId).Select(x => x.BlockedUserId).ToHashSet();
            BlockedUsers = await _userManager.GetUsersAsync(x => blockedUsers.Contains(x.Id));

            return Page();
        }
    }
}
