using AutoMapper;
using ProductService.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImage _productImageService;
        private readonly IProduct _productService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        public ProductImageController(IProduct prd, IMapper mapper, IProductImage image)
        {
            _mapper = mapper;
            _productService = prd;
            _productImageService = image;
            _response = new ResponseDto();
        }

        [HttpPost("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> AddImage(Guid Id, AddProductImageDto newImage)
        {
            var product = await _productService.GetProductById(Id);

            if (product == null)
            {
                _response.ErrorMessage = "Product Not Found";
                return NotFound(_response);
            }

            var image = _mapper.Map<ProductImage>(newImage);
            var res = await _productImageService.AddImage(Id, image);
            _response.Result = res;
            return Created("", _response);

        }
    }
}
