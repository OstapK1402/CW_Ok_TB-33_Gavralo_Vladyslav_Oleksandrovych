using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return View("Index", categories);
        }


        [Authorize(Roles = UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [Authorize(Roles = UserRole.ADMIN)]
        public async Task<ActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", category);
            }

            await _unitOfWork.CategoryRepository.CreateAsync(category);
            TempData["Success"] = "Category successfully created";
            return View("Create", category);
        }

        [Authorize(Roles = UserRole.ADMIN)]
        public async Task<IActionResult> Edit(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction("Index", "Category");
            }

            return View("Edit", category);
        }

        [Authorize(Roles = UserRole.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> Edit(int categoryId, Category categoryEdit)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Filed to edit category");
                return View("Edit", categoryEdit);
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            if (category != null)
            {
                category.Name = categoryEdit.Name;

                await _unitOfWork.CategoryRepository.Update(category);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction("Index", "Category");
            }
        }


        [Authorize(Roles = UserRole.ADMIN)]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction("Index", "Category");
            }

            await _unitOfWork.CategoryRepository.Delete(category);

            return RedirectToAction("Index", "Category");
        }
    }
}