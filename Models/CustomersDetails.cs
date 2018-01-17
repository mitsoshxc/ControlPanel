using System.ComponentModel.DataAnnotations.Schema;

namespace VPCustInfo.Models
{
    [Table("CustomersDetails")]
    public class CustomersDetails
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public int LineNo { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}