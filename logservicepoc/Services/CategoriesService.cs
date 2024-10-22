using System.ComponentModel;
using logservicepoc.DTO;
using logservicepoc.Models;

namespace logservicepoc.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDbService _dB;
        public CategoriesService(IDbService dbService)
        {
            _dB = dbService;
        }

        public async Task<ResponseObject> CreateCategory(CategoryReq categoryRequest){
            ResponseObject resObj = new ResponseObject();
            try
            {
                var category = new Categories{
                    CategoryName = categoryRequest.CategoryName,
                    Description = categoryRequest.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                var categoryId = await _dB.InsertData(
                    "INSERT INTO categories (category_name, description, created_at, updated_at) VALUES (@CategoryName, @Description, @CreatedAt, @UpdatedAt) RETURNING category_id",
                    category
                );

                category.CategoryId = categoryId;

                resObj.StatusCode = 200;
                resObj.Error = false;
                resObj.Message = "Category created successfully";
                resObj.Data = category;
            }
            catch (Exception e)
            {
                string[] errorMessage = e.Message.Split(":");
                resObj.StatusCode = 500;
                resObj.Message = errorMessage[0];
                resObj.Error = true;
            }
            return resObj;
        }

        public async Task<ResponseObject> GetAllCategories(ListReq listReq){
            ResponseObject resObj = new ResponseObject();
            try
            {
                var categories = await _dB.GetAll<dynamic>(
                    "SELECT categories.category_id as CategoryId, categories.category_name as CategoryName, categories.description as Description, categories.created_at as CreatedAt, categories.updated_at as UpdatedAt FROM categories" + " LIMIT " + listReq.Limit + " OFFSET " + listReq.Index,
                    new { }
                );
                CategoryListResponse response = new CategoryListResponse();
                response.Count = 8;

                List<CategoryResponse> categoryResponses = new List<CategoryResponse>();

                foreach( var cat in categories){
                    categoryResponses.Add(new CategoryResponse{
                        CategoryId = cat.categoryid,
                        CategoryName = cat.categoryname,
                        CategoryDescription = cat.description,
                        CreatedAt = cat.createdat,
                        UpdatedAt = cat.updatedat,
                        IsActive = true,
                    });
                }

                response.Categories = new List<CategoryResponse>();
                response.Categories = categoryResponses;

                resObj.StatusCode = 200;
                resObj.Error = false;
                resObj.Message = "Categories fetched successfully";
                resObj.Data = response;
            }
            catch (Exception e)
            {
                string[] errorMessage = e.Message.Split(":");
                resObj.StatusCode = 500;
                resObj.Message = errorMessage;
                resObj.Error = true;
            }
            return resObj;
        }
    }
}