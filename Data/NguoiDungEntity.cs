﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_HienLTH.Data
{
    [Table("NguoiDung")]
    public class NguoiDungEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }

        //
        public ICollection<UserRoleEntity> UserRoles { get; set; }    
    }
}
