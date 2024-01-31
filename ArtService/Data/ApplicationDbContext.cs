using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;
using ArtService.Models;

namespace ArtService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Art> Arts { get; set; }
    }
}
   
