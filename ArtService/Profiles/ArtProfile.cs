using AutoMapper;
using ArtService.Models.Dtos;
using ArtService.Models;

namespace ArtService.Profiles
{
    public class ArtProfile : Profile
    {

        public ArtProfile()
        {
            CreateMap<AddArtDto, Art>().ReverseMap();

        }
    }
}
