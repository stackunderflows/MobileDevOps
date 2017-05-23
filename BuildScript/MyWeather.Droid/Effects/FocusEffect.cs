using Android.Graphics;
using MyWeather.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("XVTS")]
[assembly: ExportEffect(typeof(FocusEffect), "FocusEffect")]
namespace MyWeather.Droid 
{
	public class FocusEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			Control.FocusChange += Control_FocusChange;
		}

		protected override void OnDetached()
		{
			Control.FocusChange -= Control_FocusChange;
		}

		private void Control_FocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
		{
			if (e.HasFocus)
				Control.SetBackgroundColor(Android.Graphics.Color.Yellow);
			else
				Control.SetBackgroundColor(Android.Graphics.Color.White);
		}
	}
}
