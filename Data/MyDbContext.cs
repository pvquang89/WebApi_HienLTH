using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;

namespace WebApi_HienLTH.Data
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions options) : base(options) { }


        #region dbset
        public DbSet<HangHoaEntity> HangHoas { get; set;}
        public DbSet<LoaiEntity> Loais { get; set; }


        #endregion


    }
}
