using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class TepDinhKemDanhGiaSP
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string DuongDan { get; set; }
        [Required]
        public int Loai { get; set; }
        [ForeignKey(nameof(DanhGiaSP))]
        public int IdDanhGia { get; set; }
        public DanhGiaSP DanhGiaSP { get; set; }
    }
}
