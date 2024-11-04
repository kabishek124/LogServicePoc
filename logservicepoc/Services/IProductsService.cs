using logservicepoc.DTO;

namespace logservicepoc.Services
{
    public interface IProductsService
    {
        Task<ResponseObject> CreateProduct(ProductReq productReq);
        Task<ResponseObject> GetProducts(ListReq listReq);
    }
}