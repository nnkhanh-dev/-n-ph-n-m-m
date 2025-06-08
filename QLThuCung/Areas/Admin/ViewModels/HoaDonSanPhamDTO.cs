using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using QLThuCung.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Areas.Admin.ViewModels
{
    public class HoaDonSanPhamDTO
    {
        public int Id { get; set; }
        public string IdKhachHang { get; set; }
        public string HoTenKhach { get; set; }
        public int TrangThai { get; set; }
        public int? IdPhieuGiamGia { get; set; }
        public int PhuongThucThanhToan { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string DiaChi { get; set; }
        public string NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
        public string? MaThanhToan { get; set; }
        public PhieuGiamGia PhieuGiamGia { get; set; }
        public ICollection<ChiTietHoaDonSanPham> ChiTietHoaDonSanPham { get; set; }
        public ICollection<DanhGiaSP> DanhGia { get; set; }
    }
}
