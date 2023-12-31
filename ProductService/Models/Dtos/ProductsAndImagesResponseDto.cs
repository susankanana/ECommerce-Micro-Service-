namespace ProductService.Models.Dtos
{
    public class ProductsAndImagesResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public string Availability { get; set; } = "Available";
        public List<AddProductImageDto> ProductImagesDtos { get; set; } = new List<AddProductImageDto>();
    }
}
