using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Web.Pages.Connections
{
    [Authorize]
    [BindProperties]
    public class IndexModel(ISubscriptionRepository subscriptionRepository, UserManager userManager) : PageModel
    {
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly UserManager _userManager = userManager;


        public IEnumerable<Identity.Models.User> SubscribedByUsers { get; set; } = Enumerable.Empty<Identity.Models.User>();
        public IEnumerable<Identity.Models.User> SubscribedToUsers { get; set; } = Enumerable.Empty<Identity.Models.User>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            var subscribedByUserIds = _subscriptionRepository.GetAll(x => x.ToUserId == userId).Select(x => x.FromUserId).ToHashSet();
            SubscribedByUsers = await _userManager.GetUsersAsync(x => subscribedByUserIds.Contains(x.Id));

            var subscribedToUserIds = _subscriptionRepository.GetAll(x => x.FromUserId == userId).Select(x => x.ToUserId).ToHashSet();
            SubscribedToUsers = await _userManager.GetUsersAsync(x => subscribedToUserIds.Contains(x.Id));

            return Page();
        }
    }
}
