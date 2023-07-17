using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGOModels;

namespace XGORepository.Models
{
    public class XGODbContext : DbContext
    {
        #region Fields
        private readonly IConfiguration _configuration;

        #endregion

        #region Properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shopping> Shoppings { get; set; }
        #endregion

        #region Constructors
        //public XGODbContext(DbContextOptions<XGODbContext> dbContextOptions) : base(dbContextOptions)
        //{
        //}
        public XGODbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = string.Empty;
#if DEBUG
            connectionString = "Data Source=LAPTOP-3USAUU6I\\SQLEXPRESS;Initial Catalog=XGO;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";
#else
            _configuration.GetConnectionString("azuredbConnectionstring");

#endif
            optionsBuilder.UseSqlServer(connectionString);
        }
        #endregion
    }
}
