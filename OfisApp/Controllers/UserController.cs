using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfisApp.Interfaces;
using OfisApp.Models.IdentityModels;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace OfisApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

       
        public UserController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);

                if (applicationUser != null)
                {
                    var cardNumber = applicationUser.CardNumber;
                    var userData = _userService.GetUserData(cardNumber);
                    return View(userData);
                }
                else
                {

                    return RedirectToAction("UserNotFound");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
