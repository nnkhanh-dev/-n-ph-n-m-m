using QLThuCung.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Areas.Admin.ViewModels
{
    public class SanPhamResponse
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public int? GiamGia { get; set; }
        public DateTime NgayTao { get; set; }
        public int IdDanhMuc
        {
            get; set;
        }
        public string TenDanhMuc { get; set; }
        public ICollection<AnhSanPham> AnhSanPham { get; set; }
    }
}
