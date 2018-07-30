using System;
using System.Windows;
using System.Windows.Controls;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Models.MeasurementModels;

namespace AutomatedWeatherStation.Template
{
    public class WeatherIconTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CloudyTemplate { get; set; }
        public DataTemplate LightningTemplate { get; set; }
        public DataTemplate RainyTemplate { get; set; }
        public DataTemplate SunnyTemplate { get; set; }
        public DataTemplate SunnyCloudyTemplate { get; set; }
        public DataTemplate SunnyRainyTemplate { get; set; }


        public string PropertyToEvaluate { get; set; }

        public string PropertyValueToHighlight { get; set; }

        public override DataTemplate SelectTemplate(object item,
            DependencyObject container)
        {
            var measurement = new Measurement();
            try
            {
                var mes = (MeasurementModel) item;
                if (mes == null)
                {
                    if (DateTime.Now.ToString("tt") == "AM")
                    {
                        return SunnyTemplate;
                    }
                    if (DateTime.Now.ToString("tt") == "PM")
                    {
                        return CloudyTemplate;
                    }
                }
                measurement = mes.Model;
            }
            catch (Exception)
            {
                if (DateTime.Now.ToString("tt") == "AM")
                {
                    return SunnyTemplate;
                }
                if (DateTime.Now.ToString("tt") == "PM")
                {
                    return CloudyTemplate;
                }
            }


            //// Use reﬂection to get the property to check.
            ////Type type = product.GetType();
            ////PropertyInfo property = type.GetProperty(PropertyToEvaluate);
            ////// Decide if this product should be highlighted
            ////// based on the property value.
            //if (property.GetValue(product, null).ToString() == PropertyValueToHighlight)
            ////{
            ////    return HighlightTemplate;
            ////}
            ////else
            ////{
            ////    return DefaultTemplate;
            ////}
            /// 

            try
            {
                if (measurement.Rainfall15M > 0)
                {
                    if (measurement.Date.ToString("tt") == "AM")
                    {
                        return SunnyRainyTemplate;
                    }
                    if (measurement.Date.ToString("tt") == "PM")
                    {
                        return RainyTemplate;
                    }
                }
                else
                {
                    if (measurement.Date.ToString("tt") == "AM")
                    {
                        return SunnyTemplate;
                    }
                    if (measurement.Date.ToString("tt") == "PM")
                    {
                        return CloudyTemplate;
                    }
                }
            }
            catch (Exception)
            {
                if (DateTime.Now.ToString("tt") == "AM")
                {
                    return SunnyTemplate;
                }
                if (DateTime.Now.ToString("tt") == "PM")
                {
                    return CloudyTemplate;
                }
            }
            if (DateTime.Now.ToString("tt") == "AM")
            {
                return SunnyTemplate;
            }
            if (DateTime.Now.ToString("tt") == "PM")
            {
                return CloudyTemplate;
            }
            return SunnyTemplate;
        }
    }
}