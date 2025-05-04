using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class AnhDichVu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string DuongDan { get; set; }
        [StringLength(500)]
        public string? MoTa { get; set; }
        [ForeignKey(nameof(DichVu))]
        public int IdDichVu { get; set; }
        public DichVu DichVu { get; set; }
    }
}
