namespace _04_Inventory.Api.DTOS
{
    public class MessageResponseDTO
    {
        public string? Type { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
        public object? Data { get; set; }
    }
}
