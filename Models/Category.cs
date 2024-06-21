using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        [RegularExpression("^[A-Za-z]{3}\\d{3}$")]
        public string CategoryCode { get; set; }
        public bool IsActive { get; set; }
        public List<Product> Products { get; set; }
    }
}
