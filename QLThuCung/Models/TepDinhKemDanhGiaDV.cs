using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class TepDinhKemDanhGiaDV
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string DuongDan { get; set; }
        [Required]
        public int Loai { get; set; }
        [ForeignKey(nameof(DanhGiaDV))]
        public int IdDanhGia { get; set; }
        public DanhGiaDV DanhGiaDV { get; set; }
    }
}
