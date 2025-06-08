using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class Giuong
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MaGiuong {  get; set; }
        public string? MoTa { get; set; }
        public ICollection<HoaDonDichVu> HoaDon { get; set; }
    }
}
