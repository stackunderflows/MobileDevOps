using Android.Content.Res;
using MyWeather.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System;

[assembly: Xamarin.Forms.ExportEffect(typeof(SwitchColorEffect), "SwitchColorEffect")]
namespace MyWeather.Droid
{
	public class SwitchColorEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				var switchControl = Control as Android.Support.V7.Widget.SwitchCompat;

				if (switchControl != null)
				{
					var checkBoxColor = MyWeather.SwitchColorEffect.GetColor(Element).ToAndroid();

					switchControl.ThumbDrawable.SetTint(checkBoxColor);
					switchControl.TrackDrawable.SetTint(Color.LightGray);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}
	
		protected override void OnDetached()
		{
			
		}
	}
}
