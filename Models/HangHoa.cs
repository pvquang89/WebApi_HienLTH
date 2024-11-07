using System.Text.Json.Serialization;

namespace WebApi_HienLTH.Models
{
    public class HangHoaVM
    {
        public string TenHangHoa { get; set; }
        public double DonGia { get; set; }
    }

    //về mặt database
    public class HangHoa : HangHoaVM
    {
        public Guid MaHangHoa { get; set; }
    }
}
