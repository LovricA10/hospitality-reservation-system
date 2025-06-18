using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApi.Controllers.DTOs
{
    public class MenuUpdateDTO
    {
        public int HospitalityVenueID { get; set; }
        public string? ItemName { get; set; }
        public string? ItemType { get; set; }
        public decimal? Price { get; set; }
    }
}
