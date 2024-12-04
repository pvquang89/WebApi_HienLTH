using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Data
{
    public class UserRole
    {
        //ko cần vì dùng cặp khoá tổ hợp dưới đã đủ xác định tính duy nhất mỗi dòng
        //[Key]
        //public int Id { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public NguoiDungEntity NguoiDung { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
