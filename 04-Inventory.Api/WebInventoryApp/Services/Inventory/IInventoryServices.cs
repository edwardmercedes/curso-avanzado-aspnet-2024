using WebInventoryApp.Services.DTOS;

namespace WebInventoryApp.Services.Inventory
{
    public interface IInventoryServices
    {
        #region Catetogies
        Task<string> GetAllCategories();

        Task CreateCategory(CategoryDTO category);

        Task UpdateCategory(CategoryDTO category);

        Task DeleteCategory(int code);

        #endregion

        #region productos
        Task<string> GetAllItems();

        Task CreateItems(CategoryDTO category);

        #endregion

    }
}
