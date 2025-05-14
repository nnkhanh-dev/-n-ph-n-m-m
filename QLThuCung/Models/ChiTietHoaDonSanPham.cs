using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [ValidateNever]
        public HoaDonSanPham HoaDon { get; set; }
        [ValidateNever]
        public SanPham SanPham { get; set; }
    }
}
