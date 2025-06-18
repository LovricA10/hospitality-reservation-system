using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApi.Controllers.DTOs
{
    public class MenuCreateDTO
    {
        public int HospitalityVenueID { get; set; }
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public decimal Price { get; set; }
    }
}
