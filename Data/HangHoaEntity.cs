using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyWebApiApp.Data;

namespace WebApi_HienLTH.Data
{
    [Table("HangHoa")]
    public class HangHoaEntity
    {
        [Key]
        public Guid MaHh { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenHh { get; set; }

        public string MoTa { get; set; }

        [Range(0, double.MaxValue)]
        public double DonGia { get; set; }

        public byte GiamGia { get; set; }

        //FK
        public int? MaLoai { get; set; }
        [ForeignKey("MaLoai")]
        public LoaiEntity Loai { get; set; }

        //
        public ICollection<DonHangChiTietEntity> DonHangChiTiets { get; set; }
        public HangHoaEntity()
        {
            DonHangChiTiets = new List<DonHangChiTietEntity>();
        }

    }
}
