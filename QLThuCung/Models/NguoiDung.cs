using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class NguoiDung : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string HoTen {  get; set; }
        [StringLength(200)]
        public string? DiaChi { get; set; }
        [StringLength(255)]
        public string? AnhDaiDien {  get; set; }
        [Required]
        public int KichHoat { get; set; }
        [Required]
        public int TrangThai { get; set; }
        [StringLength(500)]
        public string? GhiChu { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public GioHang GioHang { get; set; }

    }
}
