using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ReviewController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int appId)
        {
            var app = await _unitOfWork.AppRepository.GetByIdAsync(appId);

            if (app == null)
            {
                TempData["Error"] = "App not found!";
                return RedirectToAction("Index", "App");
            }

            var review = new Review
            {
                AppId = appId,
                UserId = (await _userManager.GetUserAsync(User)).Id
            };

            return View(review);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Review review)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", review);
            }

            await _unitOfWork.ReviewRepository.CreateAsync(review);
            TempData["Success"] = "Review successfully created";

            return RedirectToAction("Detail", "App", new { appId = review.AppId });
        }

        [Authorize(Roles = UserRole.USER)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var reviews = await _unitOfWork.ReviewRepository.GetReviewsByUserAsync(user.Id);

            return View("Index", reviews);
        }

        public async Task<IActionResult> Delete(int reviewId)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);

            if (review == null)
            {
                TempData["Error"] = "Review not found!";
                return RedirectToAction("Index", "Review");
            }

            await _unitOfWork.ReviewRepository.Delete(review);

            return RedirectToAction("Index", "Review");
        }
    }
}
