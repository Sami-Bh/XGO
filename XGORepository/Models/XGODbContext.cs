using Microsoft.EntityFrameworkCore;
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

        #endregion

        #region Properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shopping> Shoppings { get; set; }
        #endregion

        #region Constructors
        public XGODbContext(DbContextOptions<XGODbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public XGODbContext() 
        {
            
        }
        #endregion

        #region Methods
        #endregion
    }
}
