using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace QLThuCung.Models
{
    public class ThuCung
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, double.MaxValue)]
        public decimal CanNang { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int Tuoi { get; set; }
        [Required]
        public int GioiTinh { get; set; }
        [Required]
        [StringLength(500)]
        public string DacDiem { get; set; }
        [StringLength(255)]
        public string? AnhDaiDien { get; set; }
        [StringLength(500)]
        public string? GhiChu { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        [Required]
        public string NguoiTao {  get; set; }
        public string? NguoiCapNhat { get; set; }
        [ForeignKey(nameof(Giong))]
        public int IdGiong {  get; set; }
        [ValidateNever]
        public Giong Giong { get; set; }
        [ForeignKey(nameof(KhachHang))]
        public string IdKhachHang { get; set; }
        [ValidateNever]
        public NguoiDung KhachHang { get; set; }   
    }
}
