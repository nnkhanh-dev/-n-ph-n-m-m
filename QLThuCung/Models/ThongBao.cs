using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class ThongBao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string TieuDe { get; set; }
        [Required]
        [StringLength(500)]
        public string NoiDung { get; set; }
        [StringLength(255)]
        public string? LienKet { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayXem { get; set; }
        [ForeignKey(nameof(NguoiDung))]
        public string IdNguoiDung {get; set;}
        public NguoiDung NguoiDung { get; set; }
    }
}
