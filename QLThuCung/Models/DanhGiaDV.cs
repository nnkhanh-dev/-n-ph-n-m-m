using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class DanhGiaDV
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 5)]
        public int Sao { get; set; }
        [Required]
        [StringLength(500)]
        public string NoiDung { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        [ForeignKey(nameof(HoaDonDichVu))]
        public int IdHoaDon {  get; set; }
        public HoaDonDichVu HoaDonDichVu { get; set; }
        

    }
}
