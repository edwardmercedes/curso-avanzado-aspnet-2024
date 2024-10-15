namespace WebInventoryApp.Services.DTOS
{
    public class ItemDTO
    {
        public long Code { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Brand { get; set; }
        public double? Weight { get; set; }
        public string? Barcode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
