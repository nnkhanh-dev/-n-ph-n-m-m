using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class AnhSanPham
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string DuongDan { get; set; }
        [StringLength(500)]
        public string? MoTa { get; set; }
        [ForeignKey(nameof(SanPham))]
        public int IdSanPham { get; set; }
        public SanPham SanPham { get; set; }
    }
}
