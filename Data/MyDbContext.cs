﻿using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;

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
        }
    }
}
