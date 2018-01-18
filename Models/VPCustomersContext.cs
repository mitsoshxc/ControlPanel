using Microsoft.EntityFrameworkCore;

namespace ControlPanel.Models
{
    public class VPCustomersContext : DbContext
    {
        public VPCustomersContext(DbContextOptions<VPCustomersContext> options) : base(options) { }
        //
        // Database's tables
        //
        public DbSet<ControlPanel.Models.Users> User { get; set; }

        public DbSet<ControlPanel.Models.Customers> Customer { get; set; }

        public DbSet<ControlPanel.Models.CustomersDetails> CustomerDetails { get; set; }

        public DbSet<ControlPanel.Models.Payments> Payment { get; set; }
    }
}