using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi_HienLTH.Data
{
    public class UserRoleEntity
    {
        //ko cần vì dùng cặp khoá tổ hợp dưới đã đủ xác định tính duy nhất mỗi dòng
        //[Key]
        //public int Id { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        [JsonIgnore]
        public NguoiDungEntity NguoiDung { get; set; }

        [ForeignKey("RoleId")]
        [JsonIgnore]

        public RoleEntity Role { get; set; }
    }
}
