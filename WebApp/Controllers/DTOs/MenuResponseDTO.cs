namespace WebApp.Controllers.DTOs
{
    public class MenuResponseDTO
    {
        public int IdmenuItem { get; set; }
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public decimal Price { get; set; }
    }
}
