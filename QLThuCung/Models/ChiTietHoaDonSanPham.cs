using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class ChiTietHoaDonSanPham
    {
        public int IdHoaDon { get; set; }
        public int IdSanPham { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int SoLuong { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal DonGia { get; set; }
        public HoaDonSanPham HoaDon { get; set; }
        public SanPham SanPham { get; set; }
    }
}
