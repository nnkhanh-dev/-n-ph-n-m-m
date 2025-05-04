using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class HoaDonDichVu
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(ThuCung))]
        public int IdThuCung { get; set; }
        [Required]
        public int TrangThai { get; set; }
        [ForeignKey(nameof(PhieuGiamGia))]
        public int? IdPhieuGiamGia { get; set; }
        [Required]
        public int PhuongThucThanhToan { get; set; }
        public DateTime? BatDau { get; set; }
        public DateTime? KetThuc { get; set; }
        [Required]
        public DateTime NgayChamSoc { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int ThoiGianChamSoc { get; set; }
        [ForeignKey(nameof(DipDacBiet))]
        public int? IdDipDacBiet { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        [Required]
        public string NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
        [ValidateNever]
        public DipDacBiet DipDacBiet { get; set;}
        [ValidateNever]
        public PhieuGiamGia PhieuGiamGia { get; set ; }
        [ValidateNever]
        public ThuCung ThuCung { get; set; }
        public ICollection<ChiTietHoaDonDichVu> ChiTietHoaDonDichVu { get; set; }
    }
}
