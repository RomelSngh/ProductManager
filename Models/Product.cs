﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class Product 
    {
        public int ProductId { get ; set ; }
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get ; set ; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public Category ProductCategory { get ; set ; }
        public decimal Price { get ; set ; }
        public byte[] Image { get ; set ; }
  
    }

}
