namespace ProductService.Models.Dtos
{
    public class AddProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public string Availability { get; set; } = "Available";
    }
}
