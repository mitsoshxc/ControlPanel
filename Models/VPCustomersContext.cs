using Microsoft.EntityFrameworkCore;

namespace VPCustInfo.Models
{
    public class VPCustomersContext : DbContext
    {
        public VPCustomersContext(DbContextOptions<VPCustomersContext> options) : base(options) { }
        //
        // Database's tables
        //
        public DbSet<VPCustInfo.Models.Users> User { get; set; }

        public DbSet<VPCustInfo.Models.Customers> Customer { get; set; }

        public DbSet<VPCustInfo.Models.CustomersDetails> CustomerDetails { get; set; }
    }
}