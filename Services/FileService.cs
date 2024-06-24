using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;
using System.Drawing;

namespace ProductManagement.Services
{
    public class FileService : IFileService
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;
        
        public FileService(IProductService productService,IWebHostEnvironment environment)
        {
            _productService = productService;
            _environment = environment;
        }

        public async Task<string> ProcessUploadedExcelFileAsync(IFormFile productExcelFile)   
        {
            string rootFolder = _environment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + productExcelFile.FileName;
            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await productExcelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {                    
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null && IsValidWorkSheet(worksheet))
                    {
                        package.SaveAs(file);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            //Name	Description	CategoryName	CategoryId	Price	Image
                            var name = worksheet.Cells[row, 1].Text;
                            var description = worksheet.Cells[row, 2].Text; // Adjust column index based on your Excel file structure
                            var categoryName = worksheet.Cells[row, 3].Text;
                            var categoryId = Convert.ToInt32(worksheet.Cells[row, 4].Text);
                            var price = Convert.ToDecimal(worksheet.Cells[row, 5].Text);
                            var image = worksheet.Cells[row, 6].Text;

                            await _productService.CreateProduct(new Product { Name = name, Price = price, Description = description, CategoryName = categoryName, CategoryId = categoryId, Image = image });
                        }
                    }
                    else return "";
                }
            }
            return fileName;
        }

        private bool IsValidWorkSheet(ExcelWorksheet worksheet)
        {
            return (worksheet.Cells[1, 1].Value.ToString() == "Name") &&
                (worksheet.Cells[1, 2].Value.ToString() == "Description") &&
                (worksheet.Cells[1, 3].Value.ToString() == "CategoryName") &&
                (worksheet.Cells[1, 4].Value.ToString() == "CategoryId") &&
                (worksheet.Cells[1, 5].Value.ToString() == "Price") &&
                (worksheet.Cells[1, 6].Value.ToString() == "Image") &&
                (worksheet.Dimension.Rows > 2);
        }


        public string ProcessUploadedFile(IFormFile productImageFile)
        {
            string uniqueFileName = "";
            string path = Path.Combine(_environment.WebRootPath, "Uploads");
            //Here we create the uploads folder
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Copy the image and set the unique filename
            if (productImageFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + productImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productImageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IFormFile GetFormFile(string uniqueFileName)
        {
            string imagePath = Path.Combine(_environment.WebRootPath, "uploads", uniqueFileName);
            var mimeType = GetMimeTypeForFileExtension(imagePath);
            // Open the image file into a stream

            try
            {
                using var stream = File.OpenRead(imagePath);

                // Create a new FormFile instance from the stream
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(imagePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = mimeType // Set the MIME type according to your image file
                };
                return formFile;
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }



    }
}
