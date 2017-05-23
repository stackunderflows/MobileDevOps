using System;
using MyWeather.Models;
using Xamarin.Forms;

namespace MyWeather.DataTemplates
{
	public class ForecastDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate DefaultTemplate { get; set; }

		public DataTemplate RedTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var weatherItem = item as WeatherRoot;
			if (weatherItem != null)
			{
				if (weatherItem.MainWeather.Temperature > 10.0f)
					return RedTemplate;
			}
			
			return DefaultTemplate;
		}

	}
}
