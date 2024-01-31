using AutoMapper;
using OrderService.Models.Dtos;
using OrderService.Models;

namespace BidService.Profiles
{
    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<AddOrderDto, Order>().ReverseMap();

        }
    }
}
