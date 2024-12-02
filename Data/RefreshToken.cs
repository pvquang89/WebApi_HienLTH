using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Data
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        //FK : token tương ứng với user nào
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public NguoiDungEntity NguoiDung { get; set; }

        //nội dung token
        public string Token { get; set; }

        //access token tương ứng
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
