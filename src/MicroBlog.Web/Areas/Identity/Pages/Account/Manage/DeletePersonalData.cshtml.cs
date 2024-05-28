#nullable disable

using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Models;
using MicroBlog.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MicroBlog.Web.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel(UserManager<User> userManager, SignInManager<User> signInManager, 
        ILogger<DeletePersonalDataModel> logger, IPostRepository postRepository, IBlockRepository blockRepository, 
        ICommentRepository commentRepository, IReactionRepository reactionRepository, ISubscriptionRepository subscriptionRepository, 
        ImageService imageService) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger = logger;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IBlockRepository _blockRepository = blockRepository;
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IReactionRepository _reactionRepository = reactionRepository;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly ImageService _imageService = imageService;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            await _postRepository.DeleteForUser(user.Id);
            await _blockRepository.DeleteForUser(user.Id);
            await _commentRepository.DeleteForUser(user.Id);
            await _reactionRepository.DeleteForUser(user.Id);
            await _subscriptionRepository.DeleteForUser(user.Id);
            await _imageService.DeleteForUser(user.Id);

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
