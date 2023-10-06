using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOModels
{
    public class Category : BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        public string Name
        {
            get; set;
        }

        public virtual ObservableCollection<SubCategory> SubCategories
        {
            get; set;
        }
        #endregion

        #region Constructors
        public Category()
        {
            Name = string.Empty;
            SubCategories = new ObservableCollection<SubCategory>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
