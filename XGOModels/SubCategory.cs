using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuildingBlocks.Models;
using XGO.Store.Models;

namespace XGOModels
{
    public class SubCategory : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public string Name
        {
            get; set;
        }

        public virtual ICollection<Product> Products
        {
            get; set;
        }

        public int CategoryId { get; set; } // Required foreign key property


        public Category? Category { get; set; }        // Required reference navigation to principal
        #endregion

        #region Constructors
        public SubCategory()
        {
            Name = string.Empty;
            Products = [];
            Category = null!;
        }
        #endregion

        #region Methods

        #endregion
    }
}
