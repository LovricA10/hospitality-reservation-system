using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class MenuItemViewModel
    {
        public int IdmenuItem { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Item name can't be longer than 100 characters.")]
        [Display(Name = "Item Name")]
        public string? ItemName { get; set; }

        [Required(ErrorMessage = "Item type is required.")]
        [StringLength(50, ErrorMessage = "Item type can't be longer than 50 characters.")]
        [Display(Name = "Item Type")]
        public string? ItemType { get; set; }

        [Range(0.01, 999.99, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Venue is required.")]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [Display(Name = "Upload Image")]
        [ValidateNever]
        public IFormFile? ImageFile { get; set; }

        [ValidateNever]
        public string? ImageBase64 { get; set; }
    }
}
