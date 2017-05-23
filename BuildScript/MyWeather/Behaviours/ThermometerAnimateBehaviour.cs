using Xamarin.Forms;

namespace MyWeather
{
	public class ThermometerAnimateBehaviour : Behavior<ThermometerView>
	{
		bool isRunning;
		protected override void OnAttachedTo(ThermometerView bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.PropertyChanged += Thermometer_PropertyChanged;
		}

		protected override void OnDetachingFrom(ThermometerView bindable)
		{
			bindable.PropertyChanged -= Thermometer_PropertyChanged;
			base.OnDetachingFrom(bindable);
		}

		void Thermometer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ThermometerView.TemperatureTo))
			{
				var thermometer = (ThermometerView)sender;
			
				var animation = new Animation(v => thermometer.Temperature = (float)v, 0.0f, thermometer.TemperatureTo, Easing.BounceOut);

				animation.Commit(thermometer, "TempChange", rate: 10, length: 2000);
			}
		}
	}
}
