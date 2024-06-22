using Microsoft.AspNetCore.StaticFiles;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Models;
using System.Drawing;

namespace ProductManagement.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FileHelper(ProductDbContext context,IWebHostEnvironment environment)
        {
            this._context = context;
            this._environment = environment;
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
            using var stream = System.IO.File.OpenRead(imagePath);

            // Create a new FormFile instance from the stream
            var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(imagePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType // Set the MIME type according to your image file
            };

            return formFile;
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
