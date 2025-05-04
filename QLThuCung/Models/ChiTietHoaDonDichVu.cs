using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace QLThuCung.Models
{
    public class ChiTietHoaDonDichVu
    {
        public int IdHoaDon { get; set; }
        public int IdDichVu { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal DonGia { get; set; }
        [ValidateNever]
        public HoaDonDichVu HoaDon { get; set; }
        [ValidateNever]
        public DichVu DichVu { get; set; }
    }
}
