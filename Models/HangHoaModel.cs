using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Models
{
    public class HangHoaModel
    {
        public int MaHh { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenHh { get; set; }

        public string MoTa { get; set; }
        public byte GiamGia { get; set; }

        [Range(0, double.MaxValue)]
        public double DonGia { get; set; }
        //FK
        public int? MaLoai { get; set; }
        // Thêm tên loại
        public string TenLoai { get; set; }
    }
}
