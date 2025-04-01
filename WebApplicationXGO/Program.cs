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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddMicrosoftIdentityWebApi(builder.Configuration);
builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
#if !DEBUG
app.UseAuthentication();
app.UseAuthorization();
#endif


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
