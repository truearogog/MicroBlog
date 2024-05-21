using MicroBlog.Identity.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages
{
    [Authorize]
    public class IndexModel(UserManager userManager) : PageModel
    {
        private readonly UserManager _userManager = userManager;

        public IActionResult OnGet()
        {
            var username = _userManager.GetUserName(User);
            return Redirect($"/User/{username}");
        }
    }
}
