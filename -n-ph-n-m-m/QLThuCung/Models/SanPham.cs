using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class SanPham
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [Required]
        [StringLength(500)]
        public string MoTa { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int SoLuong { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, double.MaxValue)]
        public decimal Gia { get; set; }
        [Range(0,100)]
        public int? GiamGia { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? CapNhatBoi { get; set; }
        [ForeignKey(nameof(DanhMuc))]
        public int IdDanhMuc { get; set; }
        public DanhMuc DanhMuc { get; set; }
        public ICollection<AnhSanPham> AnhSanPham { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHang { get; set; }
        public ICollection<ChiTietHoaDonSanPham> ChiTietHoaDonSanPham { get; set;}
    }
}
