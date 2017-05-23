using MyWeather;
using MyWeather.iOS;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ThermometerView), typeof(ThermometerRenderer))]
namespace MyWeather.iOS
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
				Control.SetNeedsDisplay();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ThermometerView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				canvasView = new SKCanvasView();
				canvasView.BackgroundColor = UIColor.Clear;
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

		void PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			e.Surface.Canvas.Clear(SKColors.Transparent);
			var scale = canvasView.Window.Screen.Scale;
			therm.Temperature = Element.Temperature;
			therm.Draw(e.Surface.Canvas, (float)(Bounds.Width * scale), (float)(Bounds.Height * scale));
		}
	}
}
