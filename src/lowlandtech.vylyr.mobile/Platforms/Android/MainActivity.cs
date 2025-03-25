using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace LowlandTech.Vylyr.Mobile
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // 👇 This tells Android to resize the app when the keyboard shows
            Window.SetSoftInputMode(SoftInput.AdjustResize);
        }

    }
}
