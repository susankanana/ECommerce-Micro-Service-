﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorECommerce.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public string Availability { get; set; } = "Available";
        public List<ProductImage> productImagesDtos { get; set; } = new List<ProductImage>();
    }
    public class ProductImage
    {

        public string Image { get; set; } = string.Empty;

    }
}
