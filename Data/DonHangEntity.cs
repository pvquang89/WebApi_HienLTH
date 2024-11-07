namespace WebApi_HienLTH.Data
{
    public enum TinhTrangDonDatHang
    {
        New = 0, Payment = 1, Complete = 2, Cancel = -1
    }
    public class DonHangEntity
    {
        public Guid MaDh { get; set; }
        public DateTime NgatDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public TinhTrangDonDatHang TinhTrangDonHang { get; set; }
        public string NguoiNhan { get; set; }
        public string DiaChiGiao { get; set; }
        public string SoDienThoai { get; set; }

        //relationship
        //1 đơn hàng
        public ICollection<DonHangChiTietEntity> DonHangChiTiets { get; set; }

        public DonHangEntity()
        {
            DonHangChiTiets = new List<DonHangChiTietEntity>();
        }
    }
}
