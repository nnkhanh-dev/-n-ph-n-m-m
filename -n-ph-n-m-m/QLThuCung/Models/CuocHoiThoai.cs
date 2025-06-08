using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class CuocHoiThoai
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        [ForeignKey(nameof(KhachHang))]
        public string IdKhachHang { get; set; }
        [ForeignKey(nameof(NhanVien))]
        public string IdNhanVien { get; set; }
        public NguoiDung KhachHang { get; set; }
        public NguoiDung NhanVien { get; set; }
        public ICollection<TinNhan> TinNhan { get; set; }
    }
}
