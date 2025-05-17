using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            return View("UserProfile", user);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(User editUser)
        {
            if (editUser.UserName == null)
            {
                TempData["Error"] = "Filed to edit profile";
                return View("UserProfile", editUser);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("Logout", "Acсount");
            }

            user.UserName = editUser.UserName;

            await _unitOfWork.UserRepository.Update(user);

            return RedirectToAction("UserProfile", "Profile");
        }
    }
}
