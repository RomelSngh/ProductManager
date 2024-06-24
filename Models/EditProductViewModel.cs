﻿using ProductManagement.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class EditProductViewModel
    {
        private const string V = ".xlsx";

        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string? ImageName { get; set; }

        [Required]
        [Display(Name = "Image")]
        [AllowedExtensions(new string[] { ".jpg",".jpeg",".tiff", ".png" , ".gif" , ".bmp" })]
        public IFormFile? Image { get; set; }
    }
}
