using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Helpers;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly IFileHelper _fileHelper;
        private readonly IMapper _mapper;

        public ProductsController(ProductDbContext context,IFileHelper fileHelper,IMapper mapper)
        {
            _context = context;
            _fileHelper = fileHelper; 
            _mapper = mapper;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var productDbContext = _context.Products.Include(p => p.ProductCategory);
            return View(await productDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductCode,Name,Description,CategoryName,CategoryId,Price")] CreateProductViewModel productVm, IFormFile productFile)
        {

            var product = _mapper.Map<Product>(productVm); 

            if (ModelState.IsValid)
            {
                if (productFile != null && productFile.Length > 0)
                {
                    product.Image = _fileHelper.ProcessUploadedFile(productFile);// Process file save and get back return path
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(productVm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            var productVm = _mapper.Map<EditProductViewModel>(product);
            productVm.Image = _fileHelper.GetFormFile(productVm.ImageName);
            return View(productVm);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 50000000)] // Adjust as needed
        public async Task<IActionResult> Edit(int id,[Bind("ProductId,ProductCode,Name,Description,CategoryName,CategoryId,Price,ImageName,Image")] EditProductViewModel productVm)
        {      
            if (productVm.Image==null) //If the user hasnt uploaded a new Image
            { 
                ModelState.Remove("Image");
            }

            if (id != productVm.ProductId)
            {
                return NotFound();
            }

            var product = _mapper.Map<Product>(productVm);

            if (ModelState.IsValid)
            {
                try
                {

                    //new image scenario
                    if (productVm.Image != null && productVm.Image.Length > 0)
                    {
                        product.Image = _fileHelper.ProcessUploadedFile(productVm.Image);// Process file save and get back return path
                    }
                    //else
                    //{
                    //    productVm.Image = _fileHelper.GetFormFile(productVm.ImageName);// File Already exists
                    //    product.
                    //}

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(productVm);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProductDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

       
    }
}
