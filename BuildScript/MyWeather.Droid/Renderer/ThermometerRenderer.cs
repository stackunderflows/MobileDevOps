using MyWeather;
using MyWeather.Droid;
using SkiaSharp;
using SkiaSharp.Views.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ThermometerView), typeof(ThermometerRenderer))]
namespace MyWeather.Droid
{
	public class ThermometerRenderer : ViewRenderer<ThermometerView, SKCanvasView>
	{
		Thermometer therm;
		SKCanvasView canvasView;

		protected override void OnElementPropertyChanged(
			object sender, 
			System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(ThermometerView.Temperature))
				Control.Invalidate();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ThermometerView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				canvasView = new SKCanvasView(Context);
				therm = new Thermometer();
				SetNativeControl(canvasView);
			}

			if (e.OldElement != null)
			{
				canvasView.PaintSurface -= PaintSurface;
			}

			if (e.NewElement != null)
			{
				canvasView.PaintSurface += PaintSurface;
			}
		}

		private void PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			e.Surface.Canvas.Clear(SKColors.Transparent);
			therm.Temperature = Element.Temperature;
			therm.Draw(e.Surface.Canvas, Width, Height);
		}
	}
}
