using System.ComponentModel.DataAnnotations.Schema;

namespace VPCustInfo.Models
{
    [Table("CustomersDetails")]
    public class CustomersDetails
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public string SqlUser { get; set; }
        public string SqlPass { get; set; }
        public string WpUser { get; set; }
        public string WpPass { get; set; }
    }
}