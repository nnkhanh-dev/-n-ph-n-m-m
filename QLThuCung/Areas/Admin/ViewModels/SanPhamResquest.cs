using QLThuCung.Models;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Areas.Admin.ViewModels
{
    public class SanPhamResquest
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Ten san pham là bắt buộc")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Mo ta là bắt buộc")]
        public string MoTa { get; set; }
        [Required(ErrorMessage = "So luong là bắt buộc")]
        public int SoLuong { get; set; }
        [Required(ErrorMessage = "Gia là bắt buộc")]
        public decimal Gia { get; set; }
        public int? GiamGia { get; set; }
        public DateTime? NgayTao { get; set; }
        [Required(ErrorMessage = "Danh muc là bắt buộc")]
        public int IdDanhMuc
        {
            get; set;
        }
        public ICollection<AnhSanPham>? AnhSanPham {  get; set; }

        public List<string>? ListAnh { get; set; }
    } 
}
