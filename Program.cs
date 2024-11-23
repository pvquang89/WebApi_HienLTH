using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Helper;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<MyDbContext>
    (opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("MyDb")));


//register DI
//resgiter automapper
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddScoped<IGenericRepository<LoaiModel>, LoaiRepository>();
builder.Services.AddScoped<IGenericRepository<HangHoaModel>, HangHoaRepository>();
builder.Services.AddScoped<IGenericRepository<DonHangModel>, DonHangRepository>();
builder.Services.AddScoped<DonHangChiTietRepository>();
//
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

//for jwt
// Lấy secretKey từ file cấu hình appsettings.js
var secretKey = builder.Configuration["AppSettings:SecretKey"];
//chuyển secretKey sang dạng byte để mã hoá 
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
//đăng ký dịch vụ authen với jwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            // Tự cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            // Ký vào token
            ValidateIssuerSigningKey = true,
            //thuật toán đối xứng
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

            ClockSkew = TimeSpan.Zero //ko cho thời gian trễ, token hết hạn sẽ từ chối ngay. Mặc định 5 phút
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //đặt trước author
app.UseAuthorization();

app.MapControllers();

app.Run();
