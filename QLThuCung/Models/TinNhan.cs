using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class TinNhan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string NoiDung { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        [Required]
        public string NguoiGui { get; set; }
        [ForeignKey(nameof(CuocHoiThoai))]
        public int IdCuocHoiThoai { get; set; }
        public CuocHoiThoai CuocHoiThoai { get; set;}
    }
}
