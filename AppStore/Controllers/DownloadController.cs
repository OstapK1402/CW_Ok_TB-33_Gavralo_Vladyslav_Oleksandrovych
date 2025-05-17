using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public DownloadController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Download(int appId)
        {
            var app = await _unitOfWork.AppRepository.GetByIdAsync(appId);

            if (app == null)
            {
                TempData["Error"] = "App not found!";
                return RedirectToAction("Index", "App");
            }

            var download = new Download
            {
                AppId = appId,
                UserId = (await _userManager.GetUserAsync(User)).Id,

            };

            await _unitOfWork.DownloadRepository.CreateAsync(download);

            return RedirectToAction("Detail", "App", new { appId = appId });
        }

        public async Task<IActionResult> Delete(int downloadId)
        {
            var download = await _unitOfWork.DownloadRepository.GetByIdAsync(downloadId);

            if (download == null)
            {
                TempData["Error"] = "Download not found!";
                return RedirectToAction("Index", "App");
            }

            await _unitOfWork.DownloadRepository.Delete(download);

            return RedirectToAction("Detail", "App", new { appId = download.AppId});
        }
    }
}
