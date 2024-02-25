using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOModels.Extras
{
    public class ModulesConstants
    {
#if DEBUG
        /// <summary>
        /// The base URI for the Datasync service.
        /// </summary>
        public const string ServiceUri = "https://localhost:7001/";
#else
/// <summary>
        /// The base URI for the Datasync service.
        /// </summary>
        public const string ServiceUri = "https://xgoams.azure-api.net/";
#endif



        public const string Api = "api";
        public const string Categories = "Categories";
        public const string SubCategories = "SubCategories";
        public const string Pictures = "Pictures";
        public const string Products = "Products";
    }
}
