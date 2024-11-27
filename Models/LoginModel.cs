using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Models
{
    //tương ứng với NguoiDungEntity
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
