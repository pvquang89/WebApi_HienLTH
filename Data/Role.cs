using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Data
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }
    }
}
