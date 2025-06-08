using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuCung.Models
{
    public class Giong
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten { get; set; }
        [ForeignKey(nameof(Loai))]
        public int IdLoai { get; set; }
        [ValidateNever]
        public Loai Loai { get; set; }
    }
}
