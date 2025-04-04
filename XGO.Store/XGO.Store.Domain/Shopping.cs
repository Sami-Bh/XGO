using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Models;

namespace XGO.Store.Domain
{
    public class Shopping : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public virtual ObservableCollection<ShoppedProduct> ShoppingProducts { get; set; }
        public virtual ObservableCollection<ShoppedProduct> PurchasedProducts { get; set; }
        #endregion

        #region Constructors
        public Shopping()
        {
            ShoppingProducts = new ObservableCollection<ShoppedProduct>();
            PurchasedProducts = new ObservableCollection<ShoppedProduct>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
