using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Datasync.Client;
using Microsoft.Identity.Client;

namespace XGOMobile.Extensions
{
    public static class AuthenticationResultExtensions
    {
        public static AuthenticationToken ToAuthenticationToken(this AuthenticationResult result)
        {
            return new AuthenticationToken
            {
                DisplayName = result?.Account?.Username ?? "",
                ExpiresOn = result?.ExpiresOn ?? DateTimeOffset.MinValue,
                Token = result?.AccessToken ?? "",
                UserId = result?.Account?.Username ?? ""
            };
        }
    }
}
