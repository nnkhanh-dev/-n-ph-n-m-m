using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class DanhMuc
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [StringLength(500)]
        public string? MoTa { get; set; }
        [StringLength(255)]
        public string? AnhMinhHoa { get; set; }
        public ICollection<SanPham> SanPham { get; set; }
    }
}
