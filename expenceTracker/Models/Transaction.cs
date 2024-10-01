using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace expenceTracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;


        
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
