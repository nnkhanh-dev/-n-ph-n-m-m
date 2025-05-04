using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class DipDacBiet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [Required]
        public DateTime NgayBatDau { get; set; }
        [Required]
        public DateTime NgayKetThuc { get; set; }
        [Range(0,100)]
        public int? GiamGia { get; set; }
        [Range(0,100)]
        public int? PhuPhi { get; set; }
        public int? HoatDong { get; set; }
    }
}
