using System.Text.Json.Serialization;
using WebApi_HienLTH.Data;

namespace WebApi_HienLTH.Models
{
    //class này để thêm, sửa với những thông tin cố định 
    public class DonHangModel
    {
        public int MaDh { get; set; }
        public DateTime NgatDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        //đổi TinhTrangDonHang thành strinh thay vì enum
        public string TinhTrangDonHang { get; set; }
        public string NguoiNhan { get; set; }
        public string DiaChiGiao { get; set; }
        public string SoDienThoai { get; set; }
        //thêm mới
        //public double TongGiaTri { get; set; }
    }

    //class này dùng để hiện thị thêm 1 số thông tin nếu cần
    public class DonHangDetailModel : DonHangModel
    {
        //thêm mới
        public double TongGiaTri { get; set; }
    }
}
