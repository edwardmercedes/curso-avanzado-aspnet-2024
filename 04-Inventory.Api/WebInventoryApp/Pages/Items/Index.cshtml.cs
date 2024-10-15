using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebInventoryApp.Resources;
using WebInventoryApp.Services.Inventory;

namespace WebInventoryApp.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryServices _inventoryServices;

        public IHtmlLocalizer<Label> SharedLabels { get; private set; }

        public string DataSourceItems { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public IndexModel(IInventoryServices inventoryServices, IHtmlLocalizer<Label> sharedLabels)
        {
            _inventoryServices = inventoryServices;
            SharedLabels = sharedLabels;
        }
        public async Task<IActionResult> OnGet()
        {
            await GetRequiredDataLoadPage();
            return Page();
        }

        private async Task GetRequiredDataLoadPage()
        {
            DataSourceItems = await _inventoryServices.GetAllItems();
        }
    }
}
