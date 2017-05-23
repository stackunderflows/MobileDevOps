using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyWeather.UITest
{
    public class WeatherPage : BasePage
    {
        readonly Query locationField;
        readonly Query unitsSwitch;
        readonly Query GPSSwitch;
        readonly Query getWeatherButton;
        readonly Query temperatureLabel;
        readonly Query conditionLabel;
        readonly Query activiyIndicator;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("WeatherPageId"),
            iOS = x => x.Marked("WeatherPageId")
        };

        public WeatherPage()
        {
            locationField = x => x.Marked("LocationEntry");
            unitsSwitch = x => x.Marked("UnitsSwitch");
            GPSSwitch = x => x.Marked("GPSSwitch");
            getWeatherButton = x => x.Marked("GetWeatherButton");
            temperatureLabel = x => x.Marked("TempLabel");
            conditionLabel = x => x.Marked("ConditionLabel");
            activiyIndicator = x => x.Marked("LoadingIndicator");
        }

        public WeatherPage EnterCity(string city)
        {
            app.ClearText(locationField);
            app.EnterText(locationField, city);
            app.Screenshot($"Entered city: {city}");

            return this;
        }

        public WeatherPage ToggleUnits()
        {
            app.Tap(unitsSwitch);
            app.Screenshot("Tapped Units Switch");

            return this;
        }

        public WeatherPage ToggleGPS()
        {
            app.Tap(GPSSwitch);
            app.Screenshot("Tapped GPS Switch");

            return this;
        }

        public WeatherPage GetWeather()
        {
            app.Tap(getWeatherButton);
            app.Screenshot("Tapped Get Weather button");

            return this;
        }

        public WeatherPage VerifyImperialUnits(bool shouldBeEnabled = true)
        {
            bool isChecked = false;

            if (OnAndroid)
            {
                isChecked = app.Query(x => unitsSwitch(x).Invoke("isChecked").Value<bool>())[0];
            }

            if (OniOS)
            {
                isChecked = app.Query(x => unitsSwitch(x).Invoke("isOn").Value<int>())[0].Equals(1);
            }

            Assert.AreEqual(shouldBeEnabled, isChecked, "Imperial Switch is in the wrong position");

            return this;
        }

        public WeatherPage VerifyWeatherVisible()
        {
            app.WaitForElement(temperatureLabel, "Timed out waiting for temperature label");
            app.WaitForElement(conditionLabel, "Timed out waiting for condition label");
            app.Screenshot("Weather verified as visible");

            return this;
        }

        public WeatherPage TemperatureConditionValues(out string values)
        {
            app.WaitForNoElement(activiyIndicator, "Timed out waiting for no activity indicator");
            app.WaitForElement(temperatureLabel, "Timed out waiting for temperature label");
            app.WaitForElement(conditionLabel, "Timed out waiting for condition label");

            var temp = app.Query(temperatureLabel)[0].Text;
            var cond = app.Query(conditionLabel)[0].Text;

            values = $"Temperature: {temp}; Condition={cond}";

            return this;
        }
    }
}

