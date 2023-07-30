using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOMobile.Data
{
    public static class Constants
    {
        

        /// <summary>
        /// The application (client) ID for the native app within Azure Active Directory
        /// </summary>
        public const string ApplicationId = "02dd389d-3eff-4921-b7ce-cf45c1c870b6";

        /// <summary>
        /// The list of scopes to request
        /// </summary>
        public static string[] Scopes = new[]
        {
          "api://402b4494-e045-4154-934c-ba2200a54dc0/access_as_user"
      };
    }
}
