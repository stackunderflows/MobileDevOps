using MyWeather.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("XVTS")]
[assembly: ExportEffect(typeof(FocusEffect), "FocusEffect")]
namespace MyWeather.iOS
{
	public class FocusEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
		}
		protected override void OnDetached()
		{
		}
		protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == "IsFocused")
			{
				if ((Element as VisualElement).IsFocused)
				{
					Control.BackgroundColor = UIColor.FromRGB(180, 200, 254);
				}
				else
				{
					Control.BackgroundColor = UIColor.White;
				}
			}
		}
	}
}
