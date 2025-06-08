using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class BangGiaDV
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime NgayTao { get; set; }
        [Required]
        public int Loai {  get; set; }
        [ForeignKey(nameof(DichVu))]
        public int IdDichVu { get; set; }
        public DichVu DichVu { get; set; }
        public ICollection<ChiTietBangGiaDV> ChiTietBangGiaDV { get; set; }
    }
}
