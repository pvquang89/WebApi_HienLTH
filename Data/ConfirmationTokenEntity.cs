using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Data
{
    public class ConfirmationTokenEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }

        [ForeignKey(nameof(UserId))]
        public NguoiDungEntity NguoiDung { get; set; }
    }
}
