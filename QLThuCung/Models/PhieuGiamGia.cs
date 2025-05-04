using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class PhieuGiamGia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [Required]
        [StringLength(50)]
        public string MaGiamGia { get; set; }
        [Required]
        public DateTime NgayBatDau { get; set; }
        [Required]
        public DateTime NgayKetThuc { get; set; }
        [Required]
        [Range(0,100)]
        public int GiamGia { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int SoLuong { get; set; }
        [Required]
        public int TrangThai { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
