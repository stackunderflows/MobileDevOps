using CoreAnimation;
using MyWeather;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(GradientBoxView), typeof(GradientBoxViewRenderer))]
namespace MyWeather
{
	public class GradientBoxViewRenderer : VisualElementRenderer<BoxView>
	{
		public override void Draw(CGRect rect)
		{
			var boxView = (GradientBoxView)this.Element;
			var gradientLayer = new CAGradientLayer()
			{
				Frame = rect,	
				Colors = new CGColor[] { boxView.StartColor.ToCGColor(), boxView.EndColor.ToCGColor() }
			};
			NativeView.Layer.InsertSublayer(gradientLayer, 0);
		}
	}
}
