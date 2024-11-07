using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi_HienLTH.Data;

namespace MyWebApiApp.Data
{
    [Table("Loai")]
    public class LoaiEntity
    {
        [Key]
        public int MaLoai { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }

        public virtual ICollection<HangHoaEntity> HangHoas { get; set;}

    }
}
