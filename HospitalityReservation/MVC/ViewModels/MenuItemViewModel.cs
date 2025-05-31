namespace MVC.ViewModels
{
    public class MenuItemViewModel
    {
        public int IdmenuItem { get; set; }
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public decimal Price { get; set; }
        public int VenueId { get; set; }
    }
}
