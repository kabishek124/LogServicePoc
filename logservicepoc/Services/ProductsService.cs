using logservicepoc.DTO;
using logservicepoc.Models;

namespace logservicepoc.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IDbService _dB;
        public ProductsService(IDbService dbService)
        {
            _dB = dbService;
        }

        public async Task<ResponseObject> CreateProduct(ProductReq productReq){
            ResponseObject respObj = new ResponseObject();
            try
            {
                var product = new Products{
                    ProductName = productReq.ProductName,
                    Description = productReq.Description,
                    Price = productReq.Price,
                    StockQuantity = productReq.StockQuantity,
                    CategoryId = productReq.CategoryId,
                    CreatedByUserId = 2,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                var productId = await _dB.InsertData("INSERT INTO products(product_name, description, price, stock_quantity, category_id, created_by_user_id, is_active, created_at, updated_at) VALUES (@ProductName, @Description, @Price, @StockQuantity, @CategoryId, @CreatedByUserId, @IsActive, @CreatedAt, @UpdatedAt) RETURNING product_id", product);
                product.ProductId = productId;
                
                respObj.StatusCode = 200;
                respObj.Error = false;
                respObj.Message = "Product created successfully";
                respObj.Data = product;
            }
            catch (Exception e)
            {
                string[] errorMessage = e.Message.Split(":");
                respObj.StatusCode = 500;
                respObj.Message = errorMessage;
                respObj.Error = true;
            }
            return respObj;
        }

        public async Task<ResponseObject> GetProducts(ListReq listReq){
            ResponseObject resObj = new ResponseObject();
            try
            {
                var products = await _dB.GetAll<dynamic>("select products.product_id as ProductId, products.product_name as ProductName, products.description as Description, products.price as Price, products.stock_quantity as StockQuantity, products.category_id as CategoryId, categories.category_name as CategoryName, products.is_active as IsActive, products.created_at as CreatedAt, products.updated_at as UpdatedAt from products products LEFT JOIN categories categories ON categories.category_id = products.category_id" + " LIMIT " + listReq.Limit + " OFFSET " + listReq.Index, new{ });
                ProductListResponse listResponse = new ProductListResponse();
                listResponse.Count = 2;

                List<ProductResponse> productResponses = new List<ProductResponse>();

                foreach (var product in products)
                {
                    productResponses.Add(new ProductResponse{
                        ProductId = product.productid,
                        ProductName = product.productname,
                        Description = product.description,
                        Price = product.price,
                        StockQuantity = product.stockquantity,
                        CategoryId = product.categoryid,
                        CategoryName = product.categoryname,
                        IsActive = product.isactive,
                        CreatedAt = product.createdat,
                        UpdatedAt = product.updatedat
                    });
                }

                listResponse.Products = new List<ProductResponse>();
                listResponse.Products = productResponses;

                resObj.StatusCode = 200;
                resObj.Error = false;
                resObj.Message = "Categories fetched successfully";
                resObj.Data = listResponse;
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