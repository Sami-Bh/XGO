
using BuildingBlocks.CQRS.Generic.Queries;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.MappingProfiles;
using XGO.Storage.Api.Storage.Domain;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<XgoStorageDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
            });
            // Add services to the container.

            builder.Services.AddScoped<DbContext, XgoStorageDbContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterGenericHandlers = true;
                cfg.RegisterServicesFromAssemblyContaining(typeof(GetIList<,>));
                cfg.RegisterServicesFromAssemblyContaining(typeof(StorageLocation));// this line is for mediatr, if absent types cannot be injected

            });

            builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);


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

            using var scope = app.Services.CreateScope();
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<XgoStorageDbContext>();
                await dbContext.Database.MigrateAsync();
                await XgoStorageDbInitializer.SeedDataAsync(dbContext);
            }
            catch (Exception e)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "Error while migrating database");
            }

            app.Run();
        }
    }
}
