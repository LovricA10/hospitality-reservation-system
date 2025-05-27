using AutoMapper;
using Dao.Models;
using WebApp.Controllers.DTOs;

namespace WebApp.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // HospitalityVenue
            CreateMap<HospitalityVenue, HospitalityVenueResponseDTO>()
                    .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<HospitalityVenueCreateDTO, HospitalityVenue>();
            CreateMap<HospitalityVenueUpdateDTO, HospitalityVenue>()
                .ForMember(dest => dest.Idvenue, opt => opt.Ignore());

            // MenuItem
            CreateMap<MenuItem, MenuResponseDTO>();
            CreateMap<MenuCreateDTO, MenuItem>()
                .ForMember(dest => dest.IdmenuItem, opt => opt.Ignore());
            CreateMap<MenuUpdateDTO, MenuItem>()
                .ForMember(dest => dest.IdmenuItem, opt => opt.Ignore());

            // Reservation
            CreateMap<Reservation, ReservationResponseDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue.VenueName));
            CreateMap<ReservationCreateDTO, Reservation>()
                .ForMember(dest => dest.Idreservation, opt => opt.Ignore());
            CreateMap<ReservationUpdateDTO, Reservation>()
                .ForMember(dest => dest.Idreservation, opt => opt.Ignore());

            // User (for Register/Login, if needed)
            CreateMap<User, UserDTO>().ReverseMap();
        }
        
    
    }
}
