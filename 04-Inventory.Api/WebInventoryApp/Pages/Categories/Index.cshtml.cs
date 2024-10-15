using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebInventoryApp.Resources;
using WebInventoryApp.Services.DTOS;
using WebInventoryApp.Services.Inventory;

namespace WebInventoryApp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryServices _inventoryServices;

        public IHtmlLocalizer<Label> SharedLabels { get; private set; }

        [BindProperty]
        public string ActionType { get; set; }
        [BindProperty]
        public string ActionData { get; set; }
        public string DataSourceCategories {  get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                CategoryDTO category ;

                if (!string.IsNullOrEmpty(ActionType) && !string.IsNullOrEmpty(ActionData))
                {
                    switch (ActionType)
                    {
                        case "Edit":
                            category = JsonSerializer.Deserialize<CategoryDTO>(ActionData, options)!;
                            await _inventoryServices.UpdateCategory(category);
                            TempData["Message"] = "Registro Editado";
                            break;
                        case "Create":
                            category = JsonSerializer.Deserialize<CategoryDTO>(ActionData, options)!;
                            await _inventoryServices.CreateCategory(category!);
                            TempData["Message"] = "Registro Agregado";
                            break;
                        case "Delete":
                            var codeCategory = ActionData;
                            await _inventoryServices.DeleteCategory(Convert.ToInt32(codeCategory));
                            TempData["Message"] = "Registro Eliminado";
                            break;
                    }
                }

                await GetRequiredDataLoadPage();
                return RedirectToPage("./Index");
                //return Page();
            }
            catch (Exception ex)
            {
                switch (ActionType)
                {
                    case "Edit":
                        TempData["Message"] = "Error al editar el registro : " + ex.Message;
                        break;
                    case "Create":
                        TempData["Message"] = "Error al crear el registro";
                        break;
                    case "Delete":
                        TempData["Message"] = "Error al eliminar el registro";
                        break;
                }

            }
            return RedirectToPage("./Index");
        }


        private async Task GetRequiredDataLoadPage() 
        {
            DataSourceCategories = await _inventoryServices.GetAllCategories();
        }
    }
}
