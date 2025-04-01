using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuildingBlocks.Models;

namespace XGOModels
{
    public class Picture : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public string Description { get; set; }
        public string Infos
        {
            get; set;
        }

        public int ProductId { get; set; }

        public Product? Product { get; set; } 


        #endregion

        #region Constructors
        public Picture()
        {
            Product = null!;
            Description = string.Empty;
            Infos = string.Empty;
        }
        #endregion

        #region Methods

        #endregion
    }
}
