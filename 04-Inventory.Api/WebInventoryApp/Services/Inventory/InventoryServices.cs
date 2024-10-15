
using Microsoft.Extensions.Options;
using WebInventoryApp.Infrastructure.Inventory;
using WebInventoryApp.Services.DTOS;

namespace WebInventoryApp.Services.Inventory
{
    public class InventoryServices : IInventoryServices
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<SettingsValue> _setting;
        private readonly string _baseUrl;

        public InventoryServices(HttpClient httpClient, IOptions<SettingsValue> setting) { 
            _httpClient = httpClient;
            _setting = setting;
            _baseUrl = _setting.Value.InventoryApiUrl;
        }

        #region categories
        public async Task<string> GetAllCategories()
        {
            var uri = API.Categories.GetAllCategories(_baseUrl);
            var httpResponse = _httpClient.GetAsync(uri);   
            return await httpResponse.Result.Content.ReadAsStringAsync();
        }

        public async Task CreateCategory(CategoryDTO category)
        {
            var uri = API.Categories.CreateCategory(baseUrl:_baseUrl);
            var httpResponse = await _httpClient.PostAsJsonAsync(uri, category);
        }

        public async Task DeleteCategory(int code)
        {
            var uri = API.Categories.DeleteCategory(baseUrl: _baseUrl,code: code);
            var httpResponse = await _httpClient.DeleteAsync(uri);
        }

        public async Task UpdateCategory(CategoryDTO category)
        {
            var uri = API.Categories.UpdateCategory(baseUrl : _baseUrl);
            var httpResponse = await _httpClient.PutAsJsonAsync(uri, category);
        }
        #endregion



        #region productos
        public async Task<string> GetAllItems()
        {
            var uri = API.Items.GetAllItems(_baseUrl);
            var httpResponse = _httpClient.GetAsync(uri);
            return await httpResponse.Result.Content.ReadAsStringAsync();
        }

        public Task CreateItems(CategoryDTO category)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
