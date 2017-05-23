using SkiaSharp;

namespace MyWeather
{
	public class Thermometer
	{
		public double Temperature { get; set; }

		// Global color EdgeColor
		public static SKColor EdgeColor { get; set; } = new SKColor(102, 102, 102, 255);

		public static SKColor TempMarkerStyleFrameColor { get; set; }

		// Global color TempColor
		public static SKColor TempColor { get; set; } = new SKColor(255, 0, 0, 255);
		public static SKPaint TempMarkerStyleFillPaint { get; set; }

		// Global style ForegroundStyle
		public static SKPaint ForegroundStyleFillPaint { get; set; }
		public static SKPaint ForegroundStyleFramePaint { get; set; }

		// Global style TempStyle
		public static SKPaint TempStyleFillPaint { get; set; }
		public static SKPaint TempStyleFramePaint { get; set; }

		// Global style ShineAreaStyle
		public static SKPaint ShineAreaStyleFillPaint { get; set; }
		public static SKPaint ShineAreaStyleFramePaint { get; set; }

		/// <summary>
		/// Creates a new instance of the Untitled1 class.
		/// </summary>
		public Thermometer()
		{
			Initialize();
		}

		/// <summary>
		/// Initialize this new instance of the Untitled1 class.
		/// </summary>
		internal void Initialize()
		{
			// Fill color for ForegroundStyle
			var ForegroundStyleFillColor = new SKColor(0, 0, 0, 127);

			// Initialize ForegroundStyle fill paint
			ForegroundStyleFillPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = ForegroundStyleFillColor,
				BlendMode = SKBlendMode.SrcOver,
				IsAntialias = true
			};

			// Initialize ForegroundStyle frame paint
			ForegroundStyleFramePaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = EdgeColor,
				BlendMode = SKBlendMode.SrcOver,
				IsAntialias = true,
				StrokeWidth = 5f,
				StrokeMiter = 4f,
				StrokeJoin = SKStrokeJoin.Miter,
				StrokeCap = SKStrokeCap.Butt
			};


			// Initialize TempStyle fill paint
			TempStyleFillPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = TempColor,
				BlendMode = SKBlendMode.SrcOver,
				IsAntialias = true
			};

			// Frame color for TempStyle
			var TempStyleFrameColor = new SKColor(0, 0, 0, 255);

			// Initialize TempStyle frame paint
			TempStyleFramePaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = TempStyleFrameColor,
				BlendMode = SKBlendMode.SrcOver,
				IsAntialias = true,
				StrokeWidth = 1f,
				StrokeMiter = 4f,
				StrokeJoin = SKStrokeJoin.Miter,
				StrokeCap = SKStrokeCap.Butt
			};


			// Build ShineAreaStyleFill Blur
			var ShineAreaStyleFillBlur = SKImageFilter.CreateBlur(12f, 0f, null, null);

			// Fill color for ShineAreaStyle
			var ShineAreaStyleFillColor = new SKColor(255, 255, 255, 170);

			// Initialize ShineAreaStyle fill paint
			ShineAreaStyleFillPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = ShineAreaStyleFillColor,
				BlendMode = SKBlendMode.SrcOver,
				IsAntialias = true,
				ImageFilter = ShineAreaStyleFillBlur
			};

			TempMarkerStyleFrameColor = new SKColor(0, 0, 0, 255);

			// New TempMarker Style fill paint
			TempMarkerStyleFillPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = new SKColor(0, 0, 0, 150),
				BlendMode = SKBlendMode.SrcOver,
				IsAntialias = true,
				StrokeWidth = 2.5f
			};

		}

		/// <summary>
		/// Draw the Thermometer sketch into the given SkiaSharp canvas.
		/// </summary>
		/// <param name="canvas">The <c>SKCanvas</c> to draw the sketch into.</param>
		public void Draw(SKCanvas canvas, float width, float height)
		{
			var insets = 5f;
			var cornerSize = 40f;

			var x1 = insets;
			var y1 = insets;
			var x2 = width - insets;
			var y2 = height - insets;

			// Draw Foreground shape
			canvas.DrawRoundRect(new SKRect(x1, y1, x2, y2), cornerSize, cornerSize, ForegroundStyleFillPaint);

			// Draw TempLevel shape
			x1 += insets;
			y1 += insets;
			x2 -= insets;
			y2 -= insets;
			cornerSize -= insets;
			var rh = y2 - y1;

			if (Temperature > 0)
			{
				var tempHeight = (float)Temperature * y2;
				canvas.DrawRoundRect(new SKRect(x1, y1 + (rh - tempHeight), x2, y2), cornerSize, cornerSize, TempStyleFillPaint);
			}

			var lineSpacing = height / 10f;
			var gx1 = width * .3f;
			var gx2 = width * .7f;


			// Draw ShineArea shape
			canvas.DrawRoundRect(new SKRect(gx1, y1, gx2, y2), cornerSize, cornerSize, ShineAreaStyleFillPaint);

			// Draw Marks
			for (int i = 1; i <= 10; i++)
			{
				canvas.DrawLine(gx1, insets + (i * lineSpacing), gx2, insets + (i * lineSpacing), TempMarkerStyleFillPaint);
			}

			//Overdraw outline
			canvas.DrawRoundRect(new SKRect(x1, y1, x2, y2), cornerSize, cornerSize, ForegroundStyleFramePaint);
		}
	}
}
