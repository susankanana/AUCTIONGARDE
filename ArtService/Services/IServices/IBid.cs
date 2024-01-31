namespace ArtService.Services.IServices
{
    public interface IBid
    {
        Task<string> UpdateBidStatus(List<string> artIds);
    }
}
