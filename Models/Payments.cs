using System.ComponentModel.DataAnnotations.Schema;

namespace VPCustInfo.Models
{
    [Table("Payments")]
    public class Payments
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; } 
    }
}