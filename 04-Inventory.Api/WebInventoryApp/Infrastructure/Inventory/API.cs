namespace WebInventoryApp.Infrastructure.Inventory
{
    public class API
    {
        public static class Categories {
            public static string GetAllCategories(string baseUrl) {
                return $"{baseUrl}Categories/GetAllCategtories";
            }
            public static string CreateCategory(string baseUrl)
            {
                return $"{baseUrl}Categories/CreateCategory";
            }

            public static string UpdateCategory(string baseUrl)
            {
                return $"{baseUrl}Categories/UpdateCategory";
            }
            public static string DeleteCategory(string baseUrl,int code)
            {
                return $"{baseUrl}Categories/DeleteCategory/{code}";
            }
        }


        public static class Items
        {
            public static string GetAllItems(string baseUrl)
            {
                return $"{baseUrl}Items/GetAllItems";
            }
        }

    }
}
