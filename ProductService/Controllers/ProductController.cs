using AutoMapper;
using ProductService.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Models.Dtos;
using ProductService.Services.IServices;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        public ProductController(IProduct prd, IMapper mapper)
        {
            _mapper = mapper;
            _productService = prd;
            _response = new ResponseDto();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> AddProduct(AddProductDto newProduct)
        {
            var product = _mapper.Map<Product>(newProduct);
            var res = await _productService.AddProduct(product);
            _response.Result = res;
            return Created("", _response);

        }


        [HttpGet]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> GetProducts()
        {

            var res = await _productService.GetProducts();
            _response.Result = res;
            return Ok(_response);

        }


        [HttpGet("{Id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> GetProduct(Guid Id)
        {

            var product = await _productService.GetProductById(Id);
            if (product == null)
            {
                _response.ErrorMessage = "Product Not Found";
                return NotFound(_response);
            }
            _response.Result = product;
            return Ok(_response);

        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> UpdateProduct(Guid Id, AddProductDto updProduct)
        {
            var product = await _productService.GetProductById(Id);
            if (product == null)
            {
                _response.Result = "Product Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _mapper.Map(updProduct, product);
            var res = await _productService.UpdateProduct();
            _response.Result = res;
            return Ok(res);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteProduct(Guid Id)
        {
            var product = await _productService.GetProductById(Id);
            if (product == null)
            {
                _response.Result = "Product Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            var res = await _productService.DeleteProduct(product);
            _response.Result = res;
            return Ok(res);

        }
    }
}
