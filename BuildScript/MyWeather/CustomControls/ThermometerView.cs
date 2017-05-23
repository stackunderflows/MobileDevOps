using Xamarin.Forms;

namespace MyWeather
{
	public class ThermometerView : Xamarin.Forms.View
	{
		public static readonly BindableProperty TemperatureToProperty = BindableProperty.Create(
			propertyName: nameof(TemperatureTo),
			returnType: typeof(double),
			declaringType: typeof(ThermometerView),
			defaultValue: 0.5d);

		public static readonly BindableProperty TemperatureProperty = BindableProperty.Create(
			propertyName: nameof(Temperature),
			returnType: typeof(double),
			declaringType: typeof(ThermometerView),
			defaultValue: 0.5d);
		
		public double TemperatureTo
		{
			get { return (double)GetValue(TemperatureToProperty); }
			set { SetValue(TemperatureToProperty, value); }
		}

		public double Temperature
		{
			get { return (double)GetValue(TemperatureProperty); }
			set { SetValue(TemperatureProperty, value); }
		}
	}
}
