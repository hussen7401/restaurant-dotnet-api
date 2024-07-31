
using AutoMapper;
using core.DTOs.AccountDto;
using core.DTOs.MenuItemDto;
using core.DTOs.OrderDto;
using core.DTOs.ReservationDto;
using core.DTOs.TableDto;
using core.Entities;

namespace core.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuItemDto, MenuItem>()
                .ForMember(a => a.UpdatedAt, b => b.Ignore());

            CreateMap<OrderDto, Order>()
                .ForMember(a => a.OrderItems, b => b.Ignore())
                .ForMember(a => a.TotalAmount, b => b.Ignore());
            CreateMap<OrderItemDto, OrderItem>();


            CreateMap<Order, ShowOrder>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => new UserInfoDto
            {
                Id = src.User.Id,
                UserName = src.User.UserName,
                Email = src.User.Email
            }))
            .ForMember(dest => dest.ShowOrderItem, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderItem, ShowOrderItem>();
           

            CreateMap<User, UserDto>()
                .ForMember(a => a.Token, b => b.Ignore());

            CreateMap<TableDto, RestaurantTable>();

            CreateMap<Reservations, ReservationDto>()
           .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => new UserInfoDto
           {
               Id = src.User.Id,
               UserName = src.User.UserName,
               Email = src.User.Email
           }));

            CreateMap<Reservations, CreateReservationDto>().ReverseMap();
            CreateMap<Reservations, UpdateReservationDto>().ReverseMap();
        }
    }
}
