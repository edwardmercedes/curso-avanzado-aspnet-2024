using WebInventoryApp.Services.DTOS;

namespace WebInventoryApp.Services.Inventory
{
    public interface IInventoryServices
    {
        Task<string> GetAllCategories();

        Task CreateCategory(CategoryDTO category);

        Task UpdateCategory(CategoryDTO category);

        Task DeleteCategory(int code);

        
    }
}
