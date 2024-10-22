using logservicepoc.DTO;
using logservicepoc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace logservicepoc.Controllers
{
    [ApiController]
    [Route("category")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _catService;

        public CategoriesController (ICategoriesService service){
            _catService = service;
        }

        [HttpPost]
        [Route("category")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryReq category)
        {
            try{
                var result = await _catService.CreateCategory(category);
                return StatusCode(result.StatusCode, result);
            }catch (Exception ex){
                return StatusCode(500, new ResponseObject{
                    StatusCode = 500,
                    Error = true,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        [Route("categories")]
        public async Task<IActionResult> GetAllCategory([FromBody] ListReq listReq){
            try{
                var result = await _catService.GetAllCategories(listReq);
                return StatusCode(result.StatusCode, result);
            } catch(Exception ex){
                return StatusCode(500, new ResponseObject{
                    StatusCode = 500,
                    Error = true,
                    Message = ex.Message,
                });
            }
        }
    }
}