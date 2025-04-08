
using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
