namespace WebInventoryApp.Services.DTOS
{
    public class MessageResponseDTO
    {
        public string? Type { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
        public object? Data { get; set; }
    }
}
