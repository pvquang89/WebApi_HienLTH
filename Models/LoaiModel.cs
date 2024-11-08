using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Models
{
    public class LoaiModel
    {
        public int MaLoai { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
    }
}
