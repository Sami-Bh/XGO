using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using XGOModels.Extras;
using XGOUtilities.Constants;
using XGOUtilities.Extensions;

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

            var moduleStringToAdd = applicationModules switch
            {
                ApplicationModules.Categories => ModulesConstants.Categories,
                ApplicationModules.SubCategories => ModulesConstants.SubCategories,
                ApplicationModules.Pictures => ModulesConstants.Pictures,
                ApplicationModules.Products => ModulesConstants.Products,
                _ => throw new InvalidDataException("unknown application module"),
            };
            _finalUri += moduleStringToAdd;
            return this;
        }

        private HttpUriBuilder AddActionName(ApplicationActions applicationAction)
        {

            var moduleStringToAdd = applicationAction switch
            {
                ApplicationActions.GetByCategoryId => ActionConstants.GetByCategoryId,
                ApplicationActions.Create => ActionConstants.Create,
                _ => throw new InvalidDataException("unknown application module"),
            };

            _finalUri += moduleStringToAdd;
            return this;
        }

        private string GetFinalUri() => _finalUri;

        private HttpUriBuilder BuildControllerLevelURI(ApplicationModules applicationModules)
        {
            _finalUri = string.Empty;
            return AddApiBaseUri().AddApi().AddSlash().AddControllerName(applicationModules);
        }

        public string GetServiceURI(ApplicationModules applicationModules)
        {
            return BuildControllerLevelURI(applicationModules).GetFinalUri();
        }
        public string GetServiceURI(ApplicationModules applicationModules, ApplicationActions applicationAction)
        {
            return BuildControllerLevelURI(applicationModules).AddSlash().AddActionName(applicationAction).GetFinalUri();
        }

        public string GetServiceURI(ApplicationModules applicationModules, params (string key, string value)[] parameters)
        {
            var baseuri = GetServiceURI(applicationModules);
            string uri = AddParametersToUri(parameters, baseuri);
            return uri;
        }

        public string GetServiceURI(ApplicationModules applicationModules, ApplicationActions applicationAction, params (string key, string value)[] parameters)
        {
            var baseuri = GetServiceURI(applicationModules, applicationAction);
            string uri = AddParametersToUri(parameters, baseuri);
            return uri;
        }

        private string AddParametersToUri((string key, string value)[] parameters, string baseuri)
        {
            var query = new Dictionary<string, string>();
            foreach (var (key, value) in parameters)
            {
                query[key] = value;
            }
            var uri = QueryHelpers.AddQueryString(baseuri, query);
            return uri;
        }

        #endregion
    }
}
