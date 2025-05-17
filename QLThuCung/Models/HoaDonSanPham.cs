using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class HoaDonSanPham
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(KhachHang))]
        public string IdKhachHang { get; set; }
        [Required]
        public int TrangThai { get; set; }
        [ForeignKey(nameof(PhieuGiamGia))]
        public int? IdPhieuGiamGia { get; set; }
        [Required]
        public int PhuongThucThanhToan { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        [Required]
        public string NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
        public string? MaThanhToan { get; set; }
        [ValidateNever]
        public NguoiDung KhachHang { get; set; }
        [ValidateNever]
        public PhieuGiamGia PhieuGiamGia { get; set ; }
        public ICollection<ChiTietHoaDonSanPham> ChiTietHoaDonSanPham { get; set; }
        public ICollection<DanhGiaSP> DanhGia { get; set; }

    }
}
