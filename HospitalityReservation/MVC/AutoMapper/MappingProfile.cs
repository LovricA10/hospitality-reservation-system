
using AutoMapper;
using Dao.Models;
using MVC.ViewModels;

namespace MVC.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // HospitalityVenue
            CreateMap<HospitalityVenue, HospitalityVenueViewModel>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<HospitalityVenueViewModel, HospitalityVenue>();

            // MenuItem
            CreateMap<MenuItem, MenuItemViewModel>();
            CreateMap<MenuItemViewModel, MenuItem>();

            // Reservation
            CreateMap<Reservation, ReservationViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue.VenueName));
            CreateMap<ReservationViewModel, Reservation>();

            // User
            CreateMap<User, UserViewModel>().ReverseMap();

            // Log
            CreateMap<LogEntry, LogEntryViewModel>();
            CreateMap<LogEntryViewModel, LogEntry>();
        }
    }
}

