using System;
using MyWeather.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(SwitchColorEffect), "SwitchColorEffect")]
namespace MyWeather.iOS
{
	public class SwitchColorEffect : PlatformEffect 
	{
		protected override void OnAttached()
		{
			try
			{
				var switchControl = Control as UISwitch;

				if (switchControl != null)
				{
					switchControl.OnTintColor = MyWeather.SwitchColorEffect.GetColor(Element).ToUIColor();
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
