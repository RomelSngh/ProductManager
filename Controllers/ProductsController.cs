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
using ProductManagement.Services;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService,ICategoryService categoryService, IFileService fileService, IMapper mapper)
        {
            _categoryService = categoryService;
            _productService = productService;
            _fileService = fileService; 
            _mapper = mapper;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || await _productService.GetProducts() == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById(id);
          
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories =  _categoryService.GetCategories().GetAwaiter().GetResult();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryId");
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
                    product.Image = _fileService.ProcessUploadedFile(productFile);// Process file save and get back return path
                }

                await _productService.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetCategories();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(productVm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null ||await _productService.GetProducts() == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryService.GetCategories();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryId", product.CategoryId);
            var productVm = _mapper.Map<EditProductViewModel>(product);
            productVm.Image = _fileService.GetFormFile(productVm.ImageName);
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
                        product.Image = _fileService.ProcessUploadedFile(productVm.Image);// Process file save and get back return path
                    }

                    await _productService.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productService.ProductExists(product.ProductId))
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
            var categories = await _categoryService.GetCategories();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(productVm);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _productService.GetProducts()== null)
            {
                return NotFound();
            }

            var product =await _productService.GetProductById(id);
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
            if (await _productService.GetProducts() == null)
            {
                return Problem("Entity set 'ProductDbContext.Products'  is null.");
            }
            var product = await _productService.GetProductById(id);
            if (product != null) { 
                await _productService.DeleteProduct(product);
            }

            return RedirectToAction(nameof(Index));
        }

       
    }
}
