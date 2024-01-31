using BidService.Data;
using BidService.Models;
using BidService.Services.Iservices;
using Microsoft.EntityFrameworkCore;

namespace BidService.Services
{
    public class BidsService : IBid
    {
        private readonly ApplicationDbContext _context;
        private readonly IArt _artService;
        public BidsService(ApplicationDbContext context, IArt artService)
        {
            _context = context;
            _artService = artService;
        }

        public async Task<string> AddBid(Bid bid)
        {
            List<Bid> allBids =await GetAllBids();
            List<Bid> bidRelated= allBids.Where(e=>e.ArtId == bid.ArtId).ToList();
            int highestBid = bidRelated.Any() ? bidRelated.Max(b => b.BidAmount) : 0;
            if (bid.BidAmount >= highestBid)
            {
                bidRelated.Add(bid);
                int inclusiveHighestBid = bidRelated.Max(b => b.BidAmount);
                foreach (Bid existingBid in bidRelated)
                {
                    existingBid.HighestBid = inclusiveHighestBid;

                   
                }
                await _artService.UpdateArtHighestBid(bid.ArtId, inclusiveHighestBid);
                bid.HighestBid = inclusiveHighestBid;
                _context.Bids.Add(bid);
                await _context.SaveChangesAsync();
                return "Bid Added!!";
            }

            return "Bid amount must be greater than or equal to the highest bid.";
        }

        public async Task<string> DeleteBid(Bid bid)
        {
            // Capture the artId before removing the bid
            Guid artId = bid.ArtId;
            Bid bidRelated = await _context.Bids.FindAsync(bid.BidId);
            if (bidRelated != null)
            {
                _context.Bids.Remove(bidRelated);
                await _context.SaveChangesAsync();

                //re-calculate highest bid
                
                List<Bid> bidRelatedToArt = await _context.Bids.Where(e => e.ArtId == artId).ToListAsync();

                if (bidRelatedToArt.Count > 0)
                {
                    // Find the highest bid
                    int highestBid = bidRelatedToArt.Max(b => b.BidAmount);

                    // Update the highest bid for each bid related to the art
                    foreach (Bid existingBid in bidRelatedToArt)
                    {
                        existingBid.HighestBid = highestBid;

                        
                    }
                    await _artService.UpdateArtHighestBid(bid.ArtId, highestBid);
                    // Save changes to the database

                    await _context.SaveChangesAsync();
                }
                    return "Bid Removed!!";
            }
           return "Bid not found!!";
        }

        public async Task<List<Bid>> GetBidsByBidderId(Guid bidderId)
        {
            return await _context.Bids.Where(x => x.BidderId == bidderId).ToListAsync();
        }
        public async Task<List<Bid>> GetBidsByArtId(Guid artId)
        {
            return await _context.Bids.Where(x => x.ArtId == artId).ToListAsync();
        }

        public async Task<Bid> GetBidById(Guid Id)
        {
            return await _context.Bids.Where(x => x.BidId == Id).FirstOrDefaultAsync();
        }

        public async Task<string> UpdateBid(Bid bid)
        {
            List<Bid> allBids = await GetAllBids();
            Bid bidRelated = allBids.Find(e => e.BidId == bid.BidId);

            if (bidRelated != null)
            {
                // Detach the bidRelated entity from the context to avoid tracking conflicts
                _context.Entry(bidRelated).State = EntityState.Detached;

                // Filter bids related to the same art
                List<Bid> bidRelatedList = allBids.Where(e => e.ArtId == bid.ArtId).ToList();

                // Update the bid in the list
                int index = bidRelatedList.FindIndex(e => e.BidId == bid.BidId);
                bidRelatedList[index] = bid;

                // Recalculate highest bid
                int highestBid = bidRelatedList.Max(b => b.BidAmount);

                // Update the highest bid for each bid related to the same art
                foreach (Bid existingBid in bidRelatedList)
                {
                    existingBid.HighestBid = highestBid;

                    
                }
                await _artService.UpdateArtHighestBid(bid.ArtId, highestBid);
                // Update bid properties
                bid.HighestBid = highestBid;
                bidRelated.BidAmount = bid.BidAmount;

                // Attach the bid entity to the context
                _context.Bids.Attach(bid);

                // Mark the entity as modified
                _context.Entry(bid).State = EntityState.Modified;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return "Bid updated successfully.";
            }

            return "Bid not found.";
        }



        public async Task<List<Bid>> GetAllBids()
        {
            return await _context.Bids.ToListAsync();
        }
        public async Task<List<Bid>> GetExpiredBids()
        {
            return await _context.Bids.Where(b => b.Status == "False").ToListAsync();

        }

        public async Task<List<Bid>> GetWonBids()
        {
            var expiredBids = await GetExpiredBids();

            var wonBids = expiredBids.Where(expiredBid => expiredBid.BidAmount == expiredBid.HighestBid).ToList();

            return wonBids;

        }

        public async Task<string> UpdateBidStatus(List<string> artIds)
        {
            var bids = await GetAllBids();
            var expiredBids = bids.Where(bid => artIds.Contains(bid.ArtId.ToString()));
            if(expiredBids.Any())
            {
                foreach(var bid in expiredBids)
                {
                    bid.Status = "False";
                }
                await _context.SaveChangesAsync();
                return "Bid status updated successfully!";
            }
            return "Bid status didn't need updating :)";
        }
    }
}
