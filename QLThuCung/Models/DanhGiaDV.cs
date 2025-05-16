using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class DanhGiaDV
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Số sao đánh giá là bắt buộc.")]
        [Range(1, 5, ErrorMessage = "Số sao phải nằm trong khoảng từ 1 đến 5.")]
        public int Sao { get; set; }

        [Required(ErrorMessage = "Nội dung đánh giá không được để trống.")]
        [StringLength(500, ErrorMessage = "Nội dung đánh giá không được vượt quá 500 ký tự.")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Ngày tạo là bắt buộc.")]
        public DateTime NgayTao { get; set; }

        [ForeignKey(nameof(HoaDonDichVu))]
        public int IdHoaDon { get; set; }

        [ValidateNever]
        public HoaDonDichVu HoaDonDichVu { get; set; }
        
        public ICollection<TepDinhKemDanhGiaDV> TepDinhKem { get; set; }

    }
}
