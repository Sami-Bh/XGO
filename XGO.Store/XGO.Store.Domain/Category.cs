using BuildingBlocks.Models;

namespace XGO.Store.Domain
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

        public virtual ICollection<SubCategory> SubCategories
        {
            get; set;
        }
        #endregion

        #region Constructors
        public Category()
        {
            Name = string.Empty;
            SubCategories = [];
        }
        #endregion

        #region Methods

        #endregion
    }
}
