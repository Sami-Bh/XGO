using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Webkit;
using Microsoft.Identity.Client;
using XGOMobile.Data;
using Android.Content;

namespace XGOMobile.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
       Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
       DataHost = "auth",
       DataScheme = $"msal{Constants.ApplicationId}")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
