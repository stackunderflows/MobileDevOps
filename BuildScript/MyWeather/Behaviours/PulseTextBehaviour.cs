using System.ComponentModel;
using Xamarin.Forms;

namespace MyWeather
{
	public class PulseTextBehaviour : Behavior<Label>
	{
		protected override void OnAttachedTo(Label bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.PropertyChanged += Text_PropertyChanged;
		}

		protected override void OnDetachingFrom(Label bindable)
		{
			bindable.PropertyChanged -= Text_PropertyChanged;
			base.OnDetachingFrom(bindable);
		}

		async void Text_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Label.Text))
			{
				await ((Label)sender).ScaleTo(1.3, 250, Easing.BounceIn);
				await ((Label)sender).ScaleTo(1.0, 250, Easing.BounceOut);
			}
		}
	}
}
