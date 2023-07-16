using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Linq;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGORepository.Models;
using XGORepository.Models.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<XGODbContext>(options =>
{
    //options.UseSqlServer("Data Source=LAPTOP-3USAUU6I\\SQLEXPRESS;Initial Catalog=XGO;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
    options.UseSqlServer("Server=tcp:xgodbserver.database.windows.net,1433;Initial Catalog=xgodb;Persist Security Info=False;User ID=rsxgoadmin926;Password=AzureDb@92600;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<XGODbContext>();
await dbContext.Database.MigrateAsync();

app.Run();
