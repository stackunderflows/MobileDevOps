using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace MyWeather.UITest
{
    public class ForecastTests : BaseTestFixture
    {
        public ForecastTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        [TestCase("London, England")]
        public void GetWeatherForecast(string city)
        {
            new WeatherPage()
                .EnterCity(city)
                .GetWeather()
                .VerifyWeatherVisible()
                .GoToTab("Forecast");

            new ForecastPage()
                .ScrollDownAndUp()
                .VerifyForecastOrder();
        }
    }
}

