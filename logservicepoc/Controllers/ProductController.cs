using logservicepoc.DTO;
using logservicepoc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace logservicepoc.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] ProductReq product)
        {
            try
            {
                var res = await _productsService.CreateProduct(product);
                return StatusCode(res.StatusCode, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseObject{
                    StatusCode = 500,
                    Error = true,
                    Message = e.Message
                });
            }
        }
        [HttpPost("products")]
        // [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetProducts(ListReq listReq)
        {
            try
            {
                var res = await _productsService.GetProducts(listReq);
                return StatusCode(res.StatusCode, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResponseObject{
                    StatusCode = 500,
                    Error = true,
                    Message = e.Message
                });
            }
        }
    }
}