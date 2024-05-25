using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Extensions;
using MicroBlog.Identity.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Web.Controllers.Api
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(UserManager userManager, ISubscriptionRepository subscriptionRepository, IBlockRepository blockRepository) : Controller
    {
        private readonly UserManager _userManager = userManager;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IBlockRepository _blockRepository = blockRepository;

        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe([FromForm] string userId)
        {
            if (userId == null && !await _userManager.ExistsAsync(x => x.Id == userId))
            {
                return NotFound($"No user with id {{'{userId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _subscriptionRepository.Create(new Core.Models.Subscription { FromUserId = currentUserId, ToUserId = userId! });

            return Ok();
        }


        [HttpPost("Unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromForm] string userId)
        {
            if (userId == null && !await _userManager.ExistsAsync(x => x.Id == userId))
            {
                return NotFound($"No user with id {{'{userId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _subscriptionRepository.Delete(new Core.Models.Subscription { FromUserId = currentUserId, ToUserId = userId! });

            return Ok();
        }

        [HttpPost("Block")]
        public async Task<IActionResult> Block([FromForm] string userId)
        {
            if (userId == null && !await _userManager.ExistsAsync(x => x.Id == userId))
            {
                return NotFound($"No user with id {{'{userId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _blockRepository.Create(new Core.Models.Block { UserId = currentUserId, BlockedUserId = userId! });

            if (await _subscriptionRepository.Any(x => x.FromUserId == currentUserId && x.ToUserId == userId))
            {
                await _subscriptionRepository.Delete(new Core.Models.Subscription { FromUserId = currentUserId, ToUserId = userId! });
            }

            if (await _subscriptionRepository.Any(x => x.FromUserId == userId && x.ToUserId == currentUserId))
            {
                await _subscriptionRepository.Delete(new Core.Models.Subscription { FromUserId = userId!, ToUserId = currentUserId });
            }

            return Ok();
        }

        [HttpPost("Unblock")]
        public async Task<IActionResult> Unblock([FromForm] string userId)
        {
            if (userId == null && !await _userManager.ExistsAsync(x => x.Id == userId))
            {
                return NotFound($"No user with id {{'{userId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _blockRepository.Delete(new Core.Models.Block { UserId = currentUserId, BlockedUserId = userId! });

            return Ok();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string search, int skip, int take)
        {
            var userId = User.GetUserId();
            var users = await _userManager.Search(x => x.Id != userId && x.UserName!.StartsWith(search), skip, take);
            return PartialView("~/Pages/Shared/User/_UserList.cshtml", users);
        }
    }
}
