using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Models
{
    public class BaseModel
    {
        #region Fields

        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }
        #endregion

        #region Constructors
        public BaseModel() { }
        #endregion

        #region Methods

        #endregion
    }
}
