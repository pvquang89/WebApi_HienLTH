using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Data
{
    public class DonHangChiTietEntity
    {
        public Guid MaHh { get; set; }
        public Guid MaDh { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }

        //relationship
        public DonHangEntity DonHang { get; set; }
        public HangHoaEntity HangHoa { get; set; }
    }
}
