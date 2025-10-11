using Microsoft.EntityFrameworkCore;
using XGO.Store.Domain;

namespace XGO.Store.Persistance.Models
{
    public class XGODbContext : DbContext
    {
        #region Fields
        //private readonly IConfiguration _configuration;

        #endregion

        #region Properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shopping> Shoppings { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        #endregion

        #region Constructors
        public XGODbContext() { }

        public XGODbContext(DbContextOptions<XGODbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        //public XGODbContext(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if no options have been set (e.g., in tests)
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = string.Empty;
#if DEBUG
                connectionString = "Data Source=PHP287\\SQLEXPRESS;Initial Catalog=XGO;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True";
#else
                        _configuration.GetConnectionString("azuredbConnectionstring");

#endif
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        #endregion
    }
}
