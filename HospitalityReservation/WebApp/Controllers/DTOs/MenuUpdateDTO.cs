using System.ComponentModel.DataAnnotations;

namespace WebApp.Controllers.DTOs
{
    public class MenuUpdateDTO
    {
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public decimal Price { get; set; }
    }
}
