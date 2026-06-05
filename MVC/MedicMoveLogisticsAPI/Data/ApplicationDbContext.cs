using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsAPI.Models;

namespace MedicMoveLogisticsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

 public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}