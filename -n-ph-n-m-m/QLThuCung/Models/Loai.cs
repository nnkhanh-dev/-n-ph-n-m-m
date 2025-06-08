using System.ComponentModel.DataAnnotations;

namespace QLThuCung.Models
{
    public class Loai
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        public ICollection<Giong> Giong {  get; set; }
    }
}
