using Android.App;
using Android.Content.PM;
using Droid = Android;

namespace OnShoppingApp.Platforms.Android
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Droid.Content.Intent.ActionView },
              Categories = new[] { Droid.Content.Intent.CategoryDefault, Droid.Content.Intent.CategoryBrowsable },
              DataScheme = App.CallbackUri)]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
    }

}
