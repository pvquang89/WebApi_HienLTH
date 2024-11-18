namespace WebApi_HienLTH.Models
{
    //để update, create
    public class DonHangChiTietModel
    {
        public int MaDh { get; set; }
        public int MaHh { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }
    }

    //để get
    public class DonHangChiTietModelExtend : DonHangChiTietModel
    {
        //Thêm tên hàng hoá
        public string TenHangHoa { get; set; }
    }
}
