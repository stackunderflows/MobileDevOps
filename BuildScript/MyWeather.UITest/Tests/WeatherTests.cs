using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace MyWeather.UITest
{
    public class WeatherTests : BaseTestFixture
    {
        public WeatherTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        // Exercise 1 answer
        [Test]
        public void WeatherLocationTest()
        {
            new WeatherPage()
                .EnterCity("Seattle, USA")
                .ToggleUnits()
                .GetWeather();
        }

        // Exercise 1 bonus answer
        [Test]
        public void WeatherGPSTest()
        {
            new WeatherPage()
                .ToggleGPS()
                .GetWeather();
        }

        // Exercise 2 answer
        [Test]
        public void SetImperialUnitsTest()
        {
            new WeatherPage()
                .ToggleUnits()
                .VerifyImperialUnits();
        }

        [Test]
        [TestCase("Chicago, IL")]
        public void GetWeather(string city)
        {
            new WeatherPage()
                .EnterCity(city)
                .ToggleUnits()
                .ToggleUnits()
                .ToggleGPS()
                .ToggleGPS()
                .GetWeather()
                .VerifyWeatherVisible();
        }

        [Test]
        [TestCase("San Francisco, CA", "Chicago, IL")]
        public void UpdateWeather(string firstCity, string secondCity)
        {
            string firstValues;
            string secondValues;

            new WeatherPage()
                .EnterCity(firstCity)
                .GetWeather()
                .TemperatureConditionValues(out firstValues);

            new WeatherPage()
                .EnterCity(secondCity)
                .ToggleUnits()
                .GetWeather()
                .TemperatureConditionValues(out secondValues);

            Assert.AreNotEqual(firstValues, secondValues, "Weather forecast did not update");
        }
    }
}

