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
        public const string ApplicationId = "fdae8e39-c718-454b-83b6-e5a95c4a0d70";

        /// <summary>
        /// The list of scopes to request
        /// </summary>
        public static string[] Scopes = new[]
        {
          "api://ad0ae6a9-7b4a-476f-ba32-83855d64f41d/access_as_user"
      };
    }
}
