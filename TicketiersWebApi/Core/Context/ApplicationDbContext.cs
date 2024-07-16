using Microsoft.EntityFrameworkCore;
using TicketiersWebApi.Core.Entities;

namespace TicketiersWebApi.Core.Context
{
    
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // We need to have a DbSet for our Database Table
        public DbSet<Ticket> Tickets { get; set; }
    }
}
