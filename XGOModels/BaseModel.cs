using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOModels
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
