using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages.Post
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string id)
        {
            return Page();
        }
    }
}
