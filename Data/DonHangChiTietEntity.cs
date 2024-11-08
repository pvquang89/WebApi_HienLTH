using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Data
{
    public class DonHangChiTietEntity
    {
        public int MaHh { get; set; }
        public int MaDh { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }

        //relationship
        public DonHangEntity DonHang { get; set; }
        public HangHoaEntity HangHoa { get; set; }
    }
}
