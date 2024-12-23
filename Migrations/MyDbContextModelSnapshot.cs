﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApi_HienLTH.Data;

#nullable disable

namespace WebApi_HienLTH.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyWebApiApp.Data.LoaiEntity", b =>
                {
                    b.Property<int>("MaLoai")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MaLoai"));

                    b.Property<string>("TenLoai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("MaLoai");

                    b.ToTable("Loai");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.ConfirmationTokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ConfirmationTokens");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.DonHangChiTietEntity", b =>
                {
                    b.Property<int>("MaDh")
                        .HasColumnType("integer");

                    b.Property<int>("MaHh")
                        .HasColumnType("integer");

                    b.Property<double>("DonGia")
                        .HasColumnType("double precision");

                    b.Property<byte>("GiamGia")
                        .HasColumnType("smallint");

                    b.Property<int>("SoLuong")
                        .HasColumnType("integer");

                    b.HasKey("MaDh", "MaHh");

                    b.HasIndex("MaHh");

                    b.ToTable("ChiTietDonHang", (string)null);
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.DonHangEntity", b =>
                {
                    b.Property<int>("MaDh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MaDh"));

                    b.Property<string>("DiaChiGiao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("NgatDat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<DateTime?>("NgayGiao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NguoiNhan")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TinhTrangDonHang")
                        .HasColumnType("integer");

                    b.HasKey("MaDh");

                    b.ToTable("DonHang", (string)null);
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.HangHoaEntity", b =>
                {
                    b.Property<int>("MaHh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MaHh"));

                    b.Property<double>("DonGia")
                        .HasColumnType("double precision");

                    b.Property<byte>("GiamGia")
                        .HasColumnType("smallint");

                    b.Property<int?>("MaLoai")
                        .HasColumnType("integer");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TenHh")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("MaHh");

                    b.HasIndex("MaLoai");

                    b.ToTable("HangHoa");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.NguoiDungEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("NguoiDung");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.UserRoleEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("WebApi_HienLTH.Models.DTO.UserWithRolesDto", b =>
                {
                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("VaiTro")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.ToTable("UserWithRolesDto");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.ConfirmationTokenEntity", b =>
                {
                    b.HasOne("WebApi_HienLTH.Data.NguoiDungEntity", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.DonHangChiTietEntity", b =>
                {
                    b.HasOne("WebApi_HienLTH.Data.DonHangEntity", "DonHang")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("MaDh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_DonHangCT_DonHang");

                    b.HasOne("WebApi_HienLTH.Data.HangHoaEntity", "HangHoa")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("MaHh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_DonHangCT_HangHoa");

                    b.Navigation("DonHang");

                    b.Navigation("HangHoa");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.HangHoaEntity", b =>
                {
                    b.HasOne("MyWebApiApp.Data.LoaiEntity", "Loai")
                        .WithMany("HangHoas")
                        .HasForeignKey("MaLoai");

                    b.Navigation("Loai");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.RefreshToken", b =>
                {
                    b.HasOne("WebApi_HienLTH.Data.NguoiDungEntity", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.UserRoleEntity", b =>
                {
                    b.HasOne("WebApi_HienLTH.Data.RoleEntity", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi_HienLTH.Data.NguoiDungEntity", "NguoiDung")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MyWebApiApp.Data.LoaiEntity", b =>
                {
                    b.Navigation("HangHoas");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.DonHangEntity", b =>
                {
                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.HangHoaEntity", b =>
                {
                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.NguoiDungEntity", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("WebApi_HienLTH.Data.RoleEntity", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
