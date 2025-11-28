using EcoFoodAPI.DTOs.Product;

namespace EcoFoodAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllProductsAsync(ProductFilterDto? filter = null);
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<List<ProductResponseDto>> GetMyProductsAsync(int userId);
        Task<ProductResponseDto?> CreateProductAsync(int userId, CreateProductDto createDto);
        Task<ProductResponseDto?> UpdateProductAsync(int userId, int productId, UpdateProductDto updateDto);
        Task<bool> DeleteProductAsync(int userId, int productId, bool isAdmin = false);
    }
}