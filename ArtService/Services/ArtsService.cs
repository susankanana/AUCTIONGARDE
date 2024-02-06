using ArtService.Data;
using ArtService.Models;
using ArtService.Services.IServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArtService.Services
{
    public class ArtsService : IArt
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ArtsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> AddArt(Art art)
        {
            _context.Arts.Add(art);
            await _context.SaveChangesAsync();
            return "Art Added!!";
        }

        public async Task<string> DeleteArt(Art art)
        {
            _context.Arts.Remove(art); 
            await _context.SaveChangesAsync();
            return "Art Removed!!";
        }

        public async Task<Art> GetArtById(Guid Id)
        {
            return await _context.Arts.Where(x => x.ArtId == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Art>> GetArtsBySellerId(Guid sellerId)
        {
            return await _context.Arts.Where(x => x.SellerId == sellerId).ToListAsync();
        }

        public async Task<List<Art>> GetAllArtsStatusTrue(string status)
        {
            return await _context.Arts.Where(a => a.Status == status).ToListAsync();
        }
        public async Task<List<Art>> GetAllArts()
        {
            return await _context.Arts.ToListAsync();
        }
        public async Task<string> UpdateArtHighestBid(Guid artId, int highestBid)
        {
            var artToUpdate = await _context.Arts.Where(x=>x.ArtId == artId).FirstOrDefaultAsync();
            if (artToUpdate != null)
            {
                artToUpdate.HighestBid = highestBid;
                await _context.SaveChangesAsync();
                return "Updated successfully";
            }
            return "No update needed :)";
        }
        public async Task<string> SaveChanges()
        {
            await _context.SaveChangesAsync();
            return "Changes saved to database";
        }
        public async Task<string> UpdateArt(Art art)
        {
            var _art = await _context.Arts.Where(x=>x.ArtId == art.ArtId).FirstOrDefaultAsync();
            if (_art != null)
            {
                _mapper.Map(art, _art);
                await _context.SaveChangesAsync();
                return "Art updated successfully!!";
            }
            return "Art not found!!";
        }
    }
}
