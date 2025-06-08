using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class DichVu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int ThoiGian { get; set; }
        [Required]
        [StringLength(500)]
        public string MoTa {  get; set; }
        [Required]
        public int TrangThai { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? CapNhatBoi { get; set; }
        public ICollection<AnhDichVu> AnhDichVu { get; set; }
        public ICollection<BangGiaDV> BangGiaDV { get; set; }
        public ICollection<ChiTietHoaDonDichVu> ChiTietHoaDonDichVu { get; set; }
    }
}
