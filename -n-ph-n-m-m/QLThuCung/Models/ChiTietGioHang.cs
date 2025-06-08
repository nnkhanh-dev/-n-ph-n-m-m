using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class ChiTietGioHang
    {
        public string IdGioHang { get; set; }
        public int IdSanPham { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int SoLuong { get; set; }
        [ValidateNever]
        public GioHang GioHang { get; set; }
        [ValidateNever]
        public SanPham SanPham { get; set; }
    }
}
