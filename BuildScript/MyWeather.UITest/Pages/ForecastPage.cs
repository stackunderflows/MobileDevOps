using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyWeather.UITest
{
    public class ForecastPage : BasePage
    {
        readonly Query forecastList;
        readonly Func<int, Query> forecastDateAtIndex;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("ForecastPageId"),
            iOS = x => x.Marked("ForecastPageId")
        };

        public ForecastPage()
        {
            forecastList = x => x.Marked("ForecastListView");
            forecastDateAtIndex = (index) => x => x.Marked("ForecastCell").Index(index).Descendant().Marked("CellDateLabel");
        }

        public ForecastPage ScrollDownAndUp()
        {
            app.ScrollDown(forecastList);
            app.Screenshot("Scrolled down once");
            app.ScrollUp(forecastList);
            app.Screenshot("Scrolled up once");

            return this;
        }

        public ForecastPage VerifyForecastOrder()
        {
            var first = DateTime.Parse(app.Query(forecastDateAtIndex(0))[0].Text);
            var second = DateTime.Parse(app.Query(forecastDateAtIndex(1))[0].Text);
            Assert.IsTrue(second > first, "Not sorted from early to latest");
            app.Screenshot($"Verified sorted earliest to latest");

            return this;
        }
    }
}

