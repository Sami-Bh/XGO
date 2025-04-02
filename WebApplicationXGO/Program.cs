using System.Text.Json.Serialization;
using Application.CQRS.Product.Queries;
using Application.MappingProfiles;
using BuildingBlocks.CQRS.Generic.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using XGO.Store.Models;
using XGORepository;
using XGORepository.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.AddCors();
builder.Services.AddDbContext<XGODbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    })
    );
builder.Services.AddTransient<DbContext, XGODbContext>();//this servers for mediatr, if absent dbcontext cannot be injected


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterGenericHandlers = true;
    cfg.RegisterServicesFromAssemblyContaining(typeof(GetIList<,>));
    cfg.RegisterServicesFromAssemblyContaining(typeof(GetFilteredProducts));
    cfg.RegisterServicesFromAssemblyContaining(typeof(Category));// this line is for mediatr, if absent types cannot be injected

});

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
});

var app = builder.Build();

//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000", "https://localhost:3000"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();
using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
try
{
    await using var dbContext = serviceProvider.GetRequiredService<XGODbContext>();
    await dbContext.Database.MigrateAsync();
    await DBInitializer.SeedDataAsync(dbContext);
}
catch (Exception e)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "Error occured during migration");
}


app.Run();
