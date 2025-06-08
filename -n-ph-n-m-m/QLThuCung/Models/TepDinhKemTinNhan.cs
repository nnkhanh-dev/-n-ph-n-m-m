using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class TepDinhKemTinNhan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string DuongDan { get; set; }
        [Required]
        public int Loai { get; set; }
        [ForeignKey(nameof(TinNhan))]
        public int IdTinNhan { get; set; }
        public TinNhan TinNhan { get; set; }
    }
}
