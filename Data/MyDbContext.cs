using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using WebApi_HienLTH.Models.DTO;

namespace WebApi_HienLTH.Data
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions options) : base(options) { }


        #region dbset
        public DbSet<HangHoaEntity> HangHoas { get; set; }
        public DbSet<LoaiEntity> Loais { get; set; }
        public DbSet<DonHangEntity> DonHangs { get; set; }
        public DbSet<DonHangChiTietEntity> DonHangChiTiets { get; set; }
        public DbSet<NguoiDungEntity> NguoiDungs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<ConfirmationTokenEntity> ConfirmationTokens { get; set; }





        #endregion

        //sử dụng fluent api để tạo bảng
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHangEntity>(e =>
            {
                e.ToTable("DonHang");
                e.HasKey(dh => dh.MaDh);
                e.Property(dh => dh.NgatDat).HasDefaultValueSql("CURRENT_DATE");
                e.Property(dh => dh.NguoiNhan).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<DonHangChiTietEntity>(e =>
            {
                e.ToTable("ChiTietDonHang");
                e.HasKey(e => new { e.MaDh, e.MaHh });

                e.HasOne(e => e.DonHang) // đơn hàng chi tiết ứng với 1 đơn hàng 
                    .WithMany(e => e.DonHangChiTiets)  // Nhảy qua đơn hàng .Đơn hàng có nhiều đơn hàng chi tiết
                    .HasForeignKey(e => e.MaDh)
                    .HasConstraintName("FK_DonHangCT_DonHang");

                e.HasOne(e => e.HangHoa)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaHh)
                    .HasConstraintName("FK_DonHangCT_HangHoa");
            });

            modelBuilder.Entity<NguoiDungEntity>(e =>
            {
                e.HasIndex(e => e.UserName).IsUnique();
                e.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                e.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });
            //cấu hình composite key cho bảng UserRole
            modelBuilder.Entity<UserRoleEntity>().HasKey(ur => new { ur.UserId, ur.RoleId });

            //hasnokey : ko ánh xạ vào bảng thực tế
            modelBuilder.Entity<UserWithRolesDto>().HasNoKey();
        }
    }
}
