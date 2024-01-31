using AutoMapper;
using BidService.Models.Dtos;
using BidService.Models;

namespace BidService.Profiles
{
    public class BidProfile : Profile
    {

        public BidProfile()
        {
            CreateMap<AddBidDto, Bid>().ReverseMap();

        }
    }
}
