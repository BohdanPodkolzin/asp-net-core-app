using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace expenceTracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(50)")]
        public string Icon { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; } = "Expense";

        [NotMapped] 
        public string IconAndTitle => $"{Icon}{Title}";

        [JsonIgnore]
        public List<Transaction> Transactions { get; set; } = [];
    }
}
