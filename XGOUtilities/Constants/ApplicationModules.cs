using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOModels.Extras
{
    public enum ApplicationModules
    {
        [Description("Categories")]
        Categories,
        [Description("SubCategories")]
        SubCategories,
        [Description("Pictures")]
        Pictures,
        [Description("Products")]
        Products
    }
}
