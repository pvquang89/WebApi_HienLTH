using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Helper;
using WebApi_HienLTH.Middleware;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.JwtModel;
using WebApi_HienLTH.Models.ModelsForJwt;
using WebApi_HienLTH.Repository;
using WebApi_HienLTH.Repository.NguoiDungRepository;
using WebApi_HienLTH.Repository.Repository;

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
builder.Services.AddScoped<INguoiDungRepository, NguoiDungRepository>();


//Cấu hình làm việc với jwt
//Đọc cấu hình jwt từ appSettings.json vào jwtsettings class
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//Đăng ký DI
builder.Services.AddScoped<JwtSettings>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
// Lấy secretKey từ file cấu hình 
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
//Chuyển secretKey sang dạng byte để mã hoá 
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
//Cấu hình cơ chế authenticate jwt 
builder.Services.AddAuthentication(options =>
{
    //đăng ký dịch vụ authen
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //Ko kiểm tra nhà phát hành
        ValidateIssuer = false,
        //Ko kiểm tra người nhận 
        ValidateAudience = false,
        //Khi kích hoạt sẽ kiểm tra thời gian hiện tại tại có nằm trong khoảng nbf đến exp hay ko
        ValidateLifetime = true,
        //Kiểm tra chữ ký 
        ValidateIssuerSigningKey = true,
        //Cung cấp secret key để xác minh chữ ký (xem có trùng với chữ ký ở token người dùng cung cấp)
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
        //Ko cho thời gian trễ, token hết hạn sẽ từ chối ngay. Mặc định 5 phút
        ClockSkew = TimeSpan.Zero 
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
//app.Use(async (context, next) =>
//{
//    await next();
//    if (context.Response.StatusCode == 403)
//    {
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
//        {
//            message = "Bạn không có quyền truy cập vào tài nguyên này."
//        }));
//    }
//});


// Sử dụng middleware tùy chỉnh
app.UseCustomForbiddenMiddleware();


app.UseAuthorization();

app.MapControllers();

app.Run();
