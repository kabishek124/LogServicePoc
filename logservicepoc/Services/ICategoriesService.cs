using logservicepoc.DTO;

namespace logservicepoc.Services
{
    public interface ICategoriesService
    {
        Task<ResponseObject> CreateCategory(CategoryReq categoryRequest);
    //     Task<ResponseObject> UpdateCategory(int categoryId, CategoryRequestDTO categoryRequest);
    //     Task<ResponseObject> GetCategoryById(int categoryId);
        Task<ResponseObject> GetAllCategories(ListReq listReq);
    }
}