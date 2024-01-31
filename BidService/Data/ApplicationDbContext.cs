using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;
using BidService.Models;

namespace BidService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Bid> Bids { get; set; }
    }
}
   
