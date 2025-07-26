using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using TreasureBackEnd.Extensions;
using TreasureBackEnd.IServices;
using TreasureBackEnd.Repository;
using TreasureBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TreasureContext>(options =>
    options.UseMySql(
        "Server=database-1.c5cys2gs4q2s.ap-southeast-2.rds.amazonaws.com;Port=3306;Database=treasure;Uid=admin;Pwd=12345678;",
        new MySqlServerVersion(new Version(8, 0, 40))).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging()
                .EnableDetailedErrors()

);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});


builder.Services.AddScoped<ITreasureService, TreasureService>();


var app = builder.Build();

app.ConfigureExceptionHandler();
if (app.Environment.IsProduction())
    app.UseHsts();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
