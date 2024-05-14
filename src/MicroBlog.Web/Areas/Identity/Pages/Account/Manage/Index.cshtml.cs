#nullable disable

using MicroBlog.Identity.Extensions;
using MicroBlog.Identity.Models;
using MicroBlog.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MicroBlog.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel(UserManager<User> userManager, SignInManager<User> signInManager, ImageService imageService) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ImageService _imageService = imageService;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string ProfilePictureUrl { get; set; }

        public class InputModel
        {
            public IFormFile ProfilePicture { get; set; }
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber
            };

            ProfilePictureUrl = user.ProfilePictureUrl ?? string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            try
            {
                var profilePictureUrl = user.ProfilePictureUrl;
                if (Input.ProfilePicture != null)
                {
                    var url = await _imageService.SaveProfilePictureAsync(Input.ProfilePicture);
                    if (profilePictureUrl != null)
                    {
                        _imageService.Delete(profilePictureUrl);
                    }
                    await _userManager.SetProfilePictureAsync(user, url);
                }
            }
            catch (Exception)
            {
                StatusMessage = "Unexpected error when trying to set profile picture.";
                return RedirectToPage();
            }

            var username = await _userManager.GetUserNameAsync(user);
            if (Input.Username != username)
            {
                if (_userManager.Users.Any(x => x.UserName == Input.Username))
                {
                    StatusMessage = "Username is already taken.";
                    return RedirectToPage();
                }

                var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUsernameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set username.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
