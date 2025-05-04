using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class GioHang
    {
        [Key]
        public string IdKhachHang { get; set; }
        public NguoiDung KhachHang { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHang { get; set; }
    }
}
