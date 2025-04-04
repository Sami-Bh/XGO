using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuildingBlocks.Models;

namespace XGO.Store.Domain
{
    public class Product : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public string Name
        {
            get; set;
        }
        public string? ExtraProperties
        {
            get; set;
        }
        public bool IsProximity { get; set; }
        public bool IsHeavy { get; set; }
        public bool IsBulky { get; set; }
        public virtual ICollection<Picture> Pictures
        {
            get; set;
        }

        public int SubCategoryId { get; set; } // Required foreign key property


        public SubCategory? SubCategory { get; set; } // Required reference navigation to principal
        #endregion

        #region Constructors
        public Product()
        {
            Name = string.Empty;
            Pictures = new List<Picture>();
            SubCategory = null!;
        }
        #endregion

        #region Methods

        #endregion
    }
}
