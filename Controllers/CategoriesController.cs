using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Services;

namespace ProductManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;        

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
              return categories != null ? 
                          View(categories) :
                          Problem("Entity set 'ProductDbContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var categories = await _categoryService.GetCategories();

            if (id == null || categories == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,CategoryCode,IsActive")] Category category)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            category.CreatedBy = Guid.Parse(currentUserId);
            category.CreatedDate = DateTime.Now;

            ModelState.Remove(nameof(category.Products));

            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var categories = await _categoryService.GetCategories();
            if (id == null || categories == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,CategoryCode,IsActive")] Category category)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            category.ModifiedBy= Guid.Parse(currentUserId);
            category.ModifiedDate = DateTime.Now;

            ModelState.Remove(nameof(category.Products));

            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _categoryService.UpdateCategory(category);   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_categoryService.CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var categories = await _categoryService.GetCategories();
            if (id == null || categories == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryById(id);    
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _categoryService.GetCategories();
            if (categories == null)
            {
                return Problem("Entity set 'ProductDbContext.Categories'  is null.");
            }
            var category = await _categoryService.GetCategoryById(id);
            
            await _categoryService.DeleteCategory(category);
            return RedirectToAction(nameof(Index));
        }


    }
}
