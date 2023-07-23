using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOModels
{
    public class Product : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public SubCategory SubCategory
        {
            get; set;
        }
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
        public virtual ObservableCollection<Picture> Pictures
        {
            get; set;
        }
        #endregion

        #region Constructors
        public Product()
        {
            Pictures = new ObservableCollection<Picture>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
