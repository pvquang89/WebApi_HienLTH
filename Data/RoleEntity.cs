using System.ComponentModel.DataAnnotations;

namespace WebApi_HienLTH.Data
{
    public class RoleEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        //navigation property
        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
