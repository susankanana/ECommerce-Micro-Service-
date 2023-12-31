namespace CartService.Models.Dtos
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public string Availability { get; set; } = "Available";
    }
}
