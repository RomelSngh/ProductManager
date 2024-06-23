using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public CategoriesApiController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoriesApi
        [HttpGet("GetAllCategories")]
        [ActionName("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            return await _context.Categories.ToListAsync();
        }


        // GET: api/CategoriesApi
        [HttpGet("GetCategories")]
        [ActionName("GetCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            return await _context.Categories.Where(c=>c.IsActive==true).ToListAsync();
        }

        // GET: api/CategoriesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/CategoriesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            
            _context.Categories.Attach(category);

            // Mark all properties as modified
            _context.Entry(category).State = EntityState.Modified;

            // Exclude specified fields from being marked as modified
            _context.Entry(category).Property("CreatedBy").IsModified = false;
            _context.Entry(category).Property("CreatedDate").IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CategoriesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
          if (_context.Categories == null)
          {
              return Problem("Entity set 'ProductDbContext.Categories'  is null.");
          }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/CategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
