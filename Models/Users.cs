using System.ComponentModel.DataAnnotations.Schema;

namespace VPCustInfo.Models
{
    [Table("Users")]
    public class Users
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }
}