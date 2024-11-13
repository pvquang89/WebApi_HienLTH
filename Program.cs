using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
