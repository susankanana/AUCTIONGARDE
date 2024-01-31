using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface IArt
    {
        Task<ArtDto> GetArtById(Guid Id, string token);
    }
}
