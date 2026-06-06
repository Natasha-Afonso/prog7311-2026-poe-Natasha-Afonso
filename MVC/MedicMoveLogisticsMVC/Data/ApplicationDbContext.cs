using Microsoft.EntityFrameworkCore;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

 public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}