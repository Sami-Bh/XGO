using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using XGOMobile.Data;

namespace XGOMobile.Services.Models
{
    internal class AuthenticationService
    {
        #region Fields
        private readonly IPublicClientApplication _authenticationClient;

        #endregion

        #region Properties
        public static object ParentWindow { get; set; }

        /// <summary>
        /// Used for locking the initialization block to ensure only one initialization happens.
        /// </summary>
        private readonly SemaphoreSlim _asyncLock = new(1, 1);
        #endregion

        #region Constructors
        public AuthenticationService()
        {
#if ANDROID
        _authenticationClient = PublicClientApplicationBuilder
            .Create(Constants.ApplicationId)
            .WithAuthority(AzureCloudInstance.AzurePublic, "common")
            //.WithRedirectUri($"msal{Constants.ApplicationId}://auth")
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
            .Build();

#else
            _authenticationClient = PublicClientApplicationBuilder
                .Create(Constants.ApplicationId)
                                //.WithRedirectUri($"msal{Constants.ApplicationId}://auth")
                                .WithRedirectUri("http://localhost")
                                .Build();


#endif
        }
        #endregion

        #region Methods
        // Propagates notification that the operation should be cancelled.
        public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
        {
            AuthenticationResult result;
            try
            {
                await _asyncLock.WaitAsync();
                return await _authenticationClient
                .AcquireTokenInteractive(Constants.Scopes)
                .WithPrompt(Prompt.ForceLogin) //This is optional. If provided, on each execution, the username and the password must be entered.
#if ANDROID

                .WithParentActivityOrWindow(ParentWindow)
#elif WINDOWS
		.WithUseEmbeddedWebView(false)				
#endif
                .ExecuteAsync(cancellationToken);
                //.GetAwaiter().GetResult();
                //return result;
            }
            finally
            {
                _asyncLock.Release();

            }


        }
        #endregion
    }
}
