using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsMVC.Models;

namespace MedicMoveLogisticsMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

 public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<ServiceRequests> ServiceRequests { get; set; }
    }
}