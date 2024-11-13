namespace WebApi_HienLTH.Models
{
    public class DonHangChiTietModel
    {
        public int MaHh { get; set; }
        public int MaDh { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }

        //Thêm tên hàng hoá
        public string TenHangHoa { get; set; }
    }
}
