using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using AppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers
{
    public class ScreenshotController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ScreenshotController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> UploadScreenshot(int appId)
        {
            var app = await _unitOfWork.AppRepository.GetByIdAsync(appId);
            
            if (app == null)
            {
                TempData["Error"] = "App not found!";
                return RedirectToAction("Index", "App");
            }

            var screenshot = new UploadScreenshotViewModel
            {
                AppId = appId,
                App = app
            };

            return View(screenshot);
        }

        [HttpPost]
        public async Task<IActionResult> UploadScreenshot(UploadScreenshotViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //model.Categories = new SelectList(await _unitOfWork.CategoryRepository.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            foreach (var image in model.ImageFile)
            {
                if (image != null && image.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadsFolder = Path.Combine(wwwRootPath, "img");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileExt = Path.GetExtension(image.FileName);
                    string fileName = $"{Guid.NewGuid()}{fileExt}";
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    string dbPath = $"/img/{fileName}";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    var screenshot = new Screenshot
                    {
                        AppId = model.AppId,
                        ImageUrl = dbPath
                    };
                    await _unitOfWork.ScreenshotRepository.CreateAsync(screenshot);
                }
            }

            return RedirectToAction("Detail", "App", new { appId = model.AppId });
        }
    }
}