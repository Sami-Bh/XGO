using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Datasync.Client;
using Microsoft.Identity.Client;
using XGOMobile.Data;
using XGOMobile.Extensions;

namespace XGOMobile.Services.Models
{
    public class AuthenticationService
    {
        #region Fields
        private IPublicClientApplication _authenticationClient;

        #endregion

        #region Properties
        public static object ParentWindow { get; set; }

        public AuthenticationToken AuthenticationToken { get; set; }

        /// <summary>
        /// Used for locking the initialization block to ensure only one initialization happens.
        /// </summary>
        private readonly SemaphoreSlim _asyncLock = new(1, 1);
        #endregion

        #region Constructors
        public AuthenticationService()
        {

        }
        #endregion

        #region Methods
        private void Initialize()
        {
            _authenticationClient = PublicClientApplicationBuilder.Create(Constants.ApplicationId)
#if ANDROID
            .WithRedirectUri($"msal{Constants.ApplicationId}://auth")
                        .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#else
.WithRedirectUri("http://localhost")
#endif
            .Build();
        }

        // Propagates notification that the operation should be cancelled.
        private async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
        {
            AuthenticationResult result;
            try
            {
                await _asyncLock.WaitAsync();
                result = await _authenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .WithPrompt(Prompt.NoPrompt) //This is optional. If provided, on each execution, the username and the password must be entered.
                    .ExecuteAsync(cancellationToken);
                return result;
            }
            finally
            {
                _asyncLock.Release();

            }


        }

        private async Task<AuthenticationResult> GetRefreshedTokenAsync()
        {
            var accounts = await _authenticationClient.GetAccountsAsync();
            var result = await _authenticationClient
                .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                .ExecuteAsync();

            return result;
        }

        internal async Task<AuthenticationToken> GetToken()
        {
            if (_authenticationClient is null)
            {
                Initialize();
            }
            var interactiveMandatory = false;
            AuthenticationResult result = null;
            try
            {
                result = await GetRefreshedTokenAsync();
            }
            catch (MsalUiRequiredException)
            {
                interactiveMandatory = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MSAL Silent Error: {ex.Message}");
            }
            if (interactiveMandatory)
            {
                result = await LoginAsync(CancellationToken.None);
            }
            return result.ToAuthenticationToken();
        }
        #endregion
    }
}
