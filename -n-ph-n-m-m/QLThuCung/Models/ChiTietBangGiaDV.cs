using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class ChiTietBangGiaDV
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, double.MaxValue)]
        public decimal CanNang { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, double.MaxValue)]
        public decimal ChiPhi { get; set; }
        [ForeignKey(nameof(BangGiaDV))]
        public int IdBangGiaDV { get; set; }
        public BangGiaDV BangGiaDV { get; set;}
    }
}
