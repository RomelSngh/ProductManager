using ProductManagement.Helpers;
using System.Drawing.Printing;

namespace ProductManagement.Models
{

    public class ProductListViewModel
    {
        [AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile XlsFile { get; set; }
        public List<Product> Products { get; set; }
    }
}
