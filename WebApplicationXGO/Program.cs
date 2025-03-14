using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using WebApplicationXGO.Controllers;
using WebApplicationXGO.Services.Implementations;
using WebApplicationXGO.Services.Interfaces;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGORepository.Models;
using XGORepository.Models.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddMicrosoftIdentityWebApi(builder.Configuration);
builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();
builder.Services.AddDbContext<XGODbContext>(options =>
#if DEBUG
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    })
#else
options.UseSqlServer(Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING"), builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        })

#endif
    );

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IPictureRepository, PictureRepository>();
builder.Services.AddTransient<ISubCategoryUnitOfWork, SubCategoryUnitOfWork>();
builder.Services.AddTransient<ICategoryUnitOfWork, CategoryUnitOfWork>();
builder.Services.AddTransient<IProductUnitOfWork, ProductUnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
#if !DEBUG


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
#endif
});

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000", "https://localhost:3000"));

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
#if !DEBUG
app.UseAuthentication();
app.UseAuthorization();
#endif


app.MapControllers();

//using var scope = app.Services.CreateScope();
//await using var dbContext = scope.ServiceProvider.GetRequiredService<XGODbContext>();
//await dbContext.Database.MigrateAsync();

app.Run();
