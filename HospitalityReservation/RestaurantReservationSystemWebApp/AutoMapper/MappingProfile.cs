
using AutoMapper;
using Dao.Models;
using RestaurantReservationSystemWebApp.ViewModels;

namespace RestaurantReservationSystemWebApp.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
          
            CreateMap<HospitalityType, HospitalityTypeViewModel>().ReverseMap();

            CreateMap<HospitalityVenue, HospitalityVenueViewModel>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<HospitalityVenueViewModel, HospitalityVenue>();

            CreateMap<MenuItem, MenuItemViewModel>()
            .ForMember(dest => dest.VenueId,
        opt => opt.MapFrom(src => src.VenueMenuItems.FirstOrDefault().VenueId));
            CreateMap<MenuItemViewModel, MenuItem>();

          
            CreateMap<Reservation, ReservationViewModel>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
             .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue.VenueName));

            CreateMap<ReservationViewModel, Reservation>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Venue, opt => opt.Ignore());

            CreateMap<User, UserViewModel>().ReverseMap();

            CreateMap<LogEntry, LogEntryViewModel>();
            CreateMap<LogEntryViewModel, LogEntry>();
        }
    }
}

