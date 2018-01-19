using System.ComponentModel.DataAnnotations.Schema;

namespace ControlPanel.Models
{
    [Table("Payments")]
    public class Payments
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public int LineNo { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public double Amount { get; set; }
    }
}