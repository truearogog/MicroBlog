using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages.User
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet(string userId)
        {

        }
    }
}
