using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using AppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppStore.Controllers
{
    public class AppController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AppController(UserManager<User> userManager, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int? categoryId, string? searchTerm)
        {
            var apps = await _unitOfWork.AppRepository.GetAllAsync();

            if (categoryId.HasValue)
                apps = apps.Where(a => a.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                apps = apps.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            var viewModel = new AppFilterViewModel
            {
                CategoryId = categoryId,
                SearchTerm = searchTerm ?? string.Empty,
                Apps = apps.ToList(),
                Categories = categories.ToList()
            };

            return View("Index", viewModel);
        }

        [Authorize(Roles = UserRole.DEVELOPER)]
        public async Task<IActionResult> IndexForDeveloper ()
        {
            var user = await _userManager.GetUserAsync(User);

            var apps = await _unitOfWork.AppRepository.GetAppsByUser(user.Id);

            return View("IndexForDeveloper", apps);
        }

        public async Task<IActionResult> Detail(int appId)
        {
            var app = await _unitOfWork.AppRepository.GetByIdAsync(appId);

            if (app == null)
            {
                TempData["Error"] = "App not found!";
                return RedirectToAction("Index", "App");
            }

            return View("Detail", app);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var viewModel = new AppCreateViewModel
            {
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(await _unitOfWork.CategoryRepository.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            var app = new App
            {
                Name = model.Name,
                Description = model.Description,
                DeveloperId = (await _userManager.GetUserAsync(User)).Id,
                CategoryId = model.CategoryId,
                Price = model.Price,
                Version = model.Version,
                ReleaseDate = model.ReleaseDate,
                IsActive = model.IsActive
            };

            await _unitOfWork.AppRepository.CreateAsync(app);

            if (model.Screenshot != null && model.Screenshot.Length > 0)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string uploadsFolder = Path.Combine(wwwRootPath, "img");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileExt = Path.GetExtension(model.Screenshot.FileName);
                string fileName = $"{Guid.NewGuid()}{fileExt}";
                string filePath = Path.Combine(uploadsFolder, fileName);
                string dbPath = $"/img/{fileName}";

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Screenshot.CopyToAsync(fileStream);
                }
                var screenshot = new Screenshot
                {
                    AppId = app.Id,
                    ImageUrl = dbPath
                };
                await _unitOfWork.ScreenshotRepository.CreateAsync(screenshot);
            }

            return RedirectToAction("IndexForDeveloper", "App");
        }


        [Authorize(Roles = UserRole.DEVELOPER)]
        public async Task<IActionResult> Edit(int appId)
        {
            var app = await _unitOfWork.AppRepository.GetByIdAsync(appId);

            if (app == null)
            {
                TempData["Error"] = "App not found!";
                return RedirectToAction("IndexForDeveloper", "App");
            }

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            var editApp = new AppEditViewModel
            {
                Id = app.Id,
                Name = app.Name,
                Description = app.Description,
                CategoryId = app.CategoryId,
                IsActive = app.IsActive,
                ReleaseDate = app.ReleaseDate,
                Version = app.Version,
                Price = app.Price,
                Categories = new SelectList(categories, "Id", "Name")
            };

            return View("Edit", editApp);
        }

        [Authorize(Roles = UserRole.DEVELOPER)]
        [HttpPost]
        public async Task<IActionResult> Edit(int appId, AppEditViewModel appEdit)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Filed to edit app");
                return View("Edit", appEdit);
            }

            var app = await _unitOfWork.AppRepository.GetByIdWIncludeAsync(appId);

            if (app != null)
            {
                app.Name = appEdit.Name;
                app.Description = appEdit.Description;
                app.Price = appEdit.Price;
                app.CategoryId = appEdit.CategoryId;
                app.IsActive = appEdit.IsActive;
                app.ReleaseDate = appEdit.ReleaseDate;
                app.Version = appEdit.Version;
                app.Price = appEdit.Price;

                await _unitOfWork.AppRepository.Update(app);
                return RedirectToAction("IndexForDeveloper", "App");
            }
            else
            {
                TempData["Error"] = "App not found!";
                return RedirectToAction("IndexForDeveloper", "App");
            }
        }
    }
}
