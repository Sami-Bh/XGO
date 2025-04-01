using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Models;

namespace XGOModels
{
    public class ShoppedProduct : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        #endregion

        #region Constructors
        public ShoppedProduct() { }
        #endregion

        #region Methods

        #endregion
    }
}
