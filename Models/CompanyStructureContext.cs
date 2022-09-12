using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class CompanyStructureContext : DbContext
    {
        public DbSet<CompanyStructure> CompanyStructures { get; set; }
        public CompanyStructureContext(DbContextOptions<CompanyStructureContext> options) : base(options)
        { 
            Database.EnsureCreated();
        }
    }
}
