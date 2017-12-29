using System.ComponentModel.DataAnnotations.Schema;

namespace VPCustInfo.Models
{
    [Table("Customers")]
    public class Customers
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}