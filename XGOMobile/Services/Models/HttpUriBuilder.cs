using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGOModels.Extras;

namespace XGOMobile.Services.Models
{
    public class HttpUriBuilder
    {
        #region Fields
        private string _finalUri;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public HttpUriBuilder()
        {
            _finalUri = string.Empty;
        }
        #endregion

        #region Methods
        private HttpUriBuilder AddApiBaseUri()
        {
            _finalUri += ModulesConstants.ServiceUri;
            return this;
        }
        private HttpUriBuilder AddSlash()
        {
            _finalUri += "/";
            return this;
        }
        private HttpUriBuilder AddApi()
        {
            _finalUri += ModulesConstants.Api;
            return this;
        }

        private HttpUriBuilder AddControllerName(ApplicationModules applicationModules)
        {
            var moduleStringToAdd = "";
            switch (applicationModules)
            {
                case ApplicationModules.Categories:
                    moduleStringToAdd += ModulesConstants.Categories;
                    break;
                case ApplicationModules.SubCategories:
                    moduleStringToAdd += ModulesConstants.SubCategories;
                    break;
                case ApplicationModules.Pictures:
                    moduleStringToAdd += ModulesConstants.Pictures;
                    break;
                case ApplicationModules.Products:
                    moduleStringToAdd += ModulesConstants.Products;
                    break;
                default:
                    throw new InvalidDataException("unknown application module");
            }
            _finalUri += moduleStringToAdd;
            return this;
        }

        private string GetFinalUri() => _finalUri;
        public string GetServiceURI(ApplicationModules applicationModules) => AddApiBaseUri().AddApi().AddSlash().AddControllerName(applicationModules).GetFinalUri();
        #endregion
    }
}
