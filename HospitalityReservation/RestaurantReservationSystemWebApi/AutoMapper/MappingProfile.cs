using AutoMapper;
using Dao.Models;
using RestaurantReservationSystemWebApi.Controllers.DTOs;

namespace RestaurantReservationSystemWebApi.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HospitalityVenue, HospitalityVenueResponseDTO>()
                    .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<HospitalityVenueCreateDTO, HospitalityVenue>();
            CreateMap<HospitalityVenueUpdateDTO, HospitalityVenue>()
                .ForMember(dest => dest.Idvenue, opt => opt.Ignore());

            CreateMap<MenuItem, MenuResponseDTO>();
            CreateMap<MenuCreateDTO, MenuItem>()
                .ForMember(dest => dest.IdmenuItem, opt => opt.Ignore());
            CreateMap<MenuUpdateDTO, MenuItem>()
                .ForMember(dest => dest.IdmenuItem, opt => opt.Ignore());

            CreateMap<Reservation, ReservationResponseDTO>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
              .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue != null ? src.Venue.VenueName : string.Empty))
              .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ReservationDate)));

            CreateMap<ReservationCreateDTO, Reservation>()
                .ForMember(dest => dest.Idreservation, opt => opt.Ignore())
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate.ToDateTime(TimeOnly.MinValue)));

            CreateMap<ReservationUpdateDTO, Reservation>()
                .ForMember(dest => dest.Idreservation, opt => opt.Ignore())
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate.ToDateTime(TimeOnly.MinValue)));


            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<LogEntry, LogResponseDTO>();
            CreateMap<LogCreateDTO, LogEntry>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore());

            CreateMap<UserLoginDTO, User>();
        }


    }
}
