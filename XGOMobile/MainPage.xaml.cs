using System;
using System.Diagnostics;
using Microsoft.Datasync.Client;
using Microsoft.Identity.Client;

using XGOMobile.Data;
using XGOMobile.Services.Models;

namespace XGOMobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public IPublicClientApplication IdentityClient { get; set; }
        private readonly RemoteService _remoteService;
        public string Categorystring
        {
            get; set;
        }
        private AuthenticationToken AuthenticationToken { get; set; }
        public MainPage()
        {
            BindingContext = this;
            _remoteService = new RemoteService(GetAuthenticationToken);
            Categorystring = "default text";
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var authService = new AuthenticationService();
                authService.LoginAsync(CancellationToken.None)
                    .ContinueWith(async result =>
                    {
                        var authenticationResult = result.Result;
                        AuthenticationToken = new AuthenticationToken
                        {
                            DisplayName = authenticationResult.Account.Username,
                            ExpiresOn = authenticationResult.ExpiresOn,
                            Token = authenticationResult.AccessToken,
                            UserId = authenticationResult.TenantId
                        };


                    });
                //var result =  .GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }

            //_remoteService.GetCategory();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {

                using (var webClient = new HttpClient())
                {
                    webClient.DefaultRequestHeaders.Add("Authorization", AuthenticationToken.Token);
                    Categorystring = webClient.GetStringAsync("https://webapplicationxgo.azurewebsites.net/api/Categories/2").GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //var token = GetAuthenticationToken().Result;
            // _remoteService.GetCategory();
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        public async Task<AuthenticationToken> GetAuthenticationToken()
        {
            if (IdentityClient == null)
            {
#if ANDROID
        IdentityClient = PublicClientApplicationBuilder
            .Create(Constants.ApplicationId)
            .WithAuthority(AzureCloudInstance.AzurePublic, "common")
            .WithRedirectUri($"msal{Constants.ApplicationId}://auth")
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
            .Build();
#else
                IdentityClient = PublicClientApplicationBuilder
                    .Create(Constants.ApplicationId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                    .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                    .Build();
#endif
            }

            var accounts = await IdentityClient.GetAccountsAsync();
            AuthenticationResult result = null;
            bool tryInteractiveLogin = false;

            try
            {
                result = await IdentityClient
                    .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                tryInteractiveLogin = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MSAL Silent Error: {ex.Message}");
            }

            if (tryInteractiveLogin)
            {
                try
                {
                    result = await IdentityClient
                        .AcquireTokenInteractive(Constants.Scopes)
#if ANDROID
                .WithParentActivityOrWindow(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity)
#endif
#if WINDOWS
		.WithUseEmbeddedWebView(false)				
#endif
                        .ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"MSAL Interactive Error: {ex.Message}");
                }
            }

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