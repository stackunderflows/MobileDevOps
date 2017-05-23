using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MyWeather.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
    {
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
#if DEBUG
			Xamarin.Calabash.Start(); 
#endif

            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(1, 136, 254);  //#58F//bar background
            UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("HelveticaNeue-Light", 20f),
                TextColor = UIColor.White
            });

            Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

		public override void OnResignActivation (UIApplication application)
		{
		}

		public override void DidEnterBackground (UIApplication application)
		{
		}

		public override void WillEnterForeground (UIApplication application)
		{
		}

		public override void OnActivated (UIApplication application)
		{
		}

		public override void WillTerminate (UIApplication application)
		{
		}
	}
}


