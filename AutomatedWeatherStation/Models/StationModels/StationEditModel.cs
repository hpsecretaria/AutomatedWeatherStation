using System;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Models.Editable;
using static System.Decimal;

namespace AutomatedWeatherStation.Models.StationModels
{
    public class NewStationModel : StationEditModel
    {
        public NewStationModel() : base(new Station())
        {
            InitializeRequiredFields();
        }

        private void InitializeRequiredFields()
        {
            Location = string.Empty;
            DisplayLocation = string.Empty;
            DisplayLocationArea = string.Empty;
            Imei = string.Empty;
            PhoneNumber = string.Empty;
            Keyword = string.Empty;
            Latitude = 0;
            Longitude = 0;
            LatitudeString = string.Empty;
            LongitudeString = string.Empty;
        }
    }

    public class StationEditModel : EditModelBase<Station>
    {
        private string _latitudeString; private string _longitudeString;

        protected StationEditModel() : base(new Station())
        {
        }

        public StationEditModel(Station model) : base(model)
        {
            ModelCopy = CreateCopy(model);
            LatitudeString = model.Latitude.ToString();
            LongitudeString = model.Longitude.ToString();
        }

        public string Location
        {
            get { return _ModelCopy.Location; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Location),
                    () => string.IsNullOrWhiteSpace(value), "Location should not be empty");
                _ModelCopy.Location = newValue;
            }
        }

        public string DisplayLocation
        {
            get { return _ModelCopy.DisplayLocation; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(DisplayLocation),
                    () => string.IsNullOrWhiteSpace(value), "City/Province should not be empty");

                _ModelCopy.DisplayLocation = newValue;
            }
        }

        public string DisplayLocationArea
        {
            get { return _ModelCopy.DisplayLocationArea; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(DisplayLocationArea),
                    () => string.IsNullOrWhiteSpace(value), "Area should not be empty");

                _ModelCopy.DisplayLocationArea = newValue;
            }
        }

        public string Keyword
        {
            get { return _ModelCopy.Keyword; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Keyword),
                    () => string.IsNullOrWhiteSpace(value), "Keyword should not be empty");

                _ModelCopy.Keyword = newValue;
            }
        }

        public string PhoneNumber
        {
            get { return _ModelCopy.PhoneNumber; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(PhoneNumber),
                    () =>
                    {
                        long x;
                        var result = long.TryParse(value, out x);
                        return !result;
                    }, "Phone number should not contain letters");

                _ModelCopy.PhoneNumber = newValue;
            }
        }

        public string Imei
        {
            get { return _ModelCopy.Imei; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Imei),
                    () =>
                    {
                        long x;
                        var result = long.TryParse(value, out x);

                      

                        return !result;
                    }, "IMEI should not contain letters");

                _ModelCopy.Imei = newValue;
            }
        }

        public string LatitudeString
        {
            get { return _latitudeString; }
            set
            {
                var tmp = value;

                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(LatitudeString),
                    () =>
                    {
                        decimal x;
                        var result = TryParse(value, out x);

                        //if (string.IsNullOrEmpty(value))
                        //{
                        //    _ModelCopy.Latitude = 0;
                        //    return false;
                        //}

                        if (result)
                        {
                            _ModelCopy.Latitude = Convert.ToDecimal(
                                value);
                        }
                        return !result;
                    }, "Latitude should not contain letters");


                if (string.IsNullOrEmpty(value))
                {
                    var newValue2 = ValidateInputAndAddErrors(ref tmp, value, nameof(LatitudeString),
                () =>
                {


                    if (string.IsNullOrEmpty(value))
                    {
                        _ModelCopy.Latitude = 0;
                        return true;
                    }
                    return false;


                }, "Latitude should not be empty");
                }
            

                _latitudeString = value;
                
            }
        }

       
        public string LongitudeString
        {
            get { return _longitudeString; }
            set
            {
                var tmp = value;
                var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(LongitudeString),
                () =>
                {
                    decimal x;
                    var result = TryParse(value, out x);

                        //if (string.IsNullOrEmpty(value))
                        //{
                        //    _ModelCopy.Latitude = 0;
                        //    return false;
                        //}

                        if (result)
                    {
                        _ModelCopy.Longitude = Convert.ToDecimal(
                            value);
                    }
                    return !result;
                }, "Longitude should not contain letters");


                if (string.IsNullOrEmpty(value))
                {
                    var newValue2 = ValidateInputAndAddErrors(ref tmp, value, nameof(LongitudeString),
                () =>
                {


                    if (string.IsNullOrEmpty(value))
                    {
                        _ModelCopy.Longitude = 0;
                        return true;
                    }
                    return false;


                }, "Longitude should not be empty");
                }

                _longitudeString = value;
            }
        }

        public decimal? Latitude
        {
            get { return _ModelCopy.Latitude; }
            set
            {
                _ModelCopy.Latitude = value;
                //var tmp = value;
                
                //var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Latitude),
                //    () =>
                //    {
                //        long x;
                //        var s = value.ToString();
                //        var result = long.TryParse(s, out x);
                //        return !result;
                //    }, "Latitude should not contain letters");

                //_ModelCopy.Latitude = newValue;
            }
        }

        public decimal? Longitude
        {
            get { return _ModelCopy.Longitude; }
            set
            {
                _ModelCopy.Longitude = value;
                //var tmp = value;
                //var newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Longitude),
                //    () =>
                //    {
                //        try
                //        {
                //            decimal o;
                //            return !decimal.TryParse(value.ToString(),out o);
                //        }
                //        catch (Exception)
                //        {
                //            return false;
                //        }
                //    }, "Longitude should not contain letters");

                //_ModelCopy.Latitude = newValue;
            }
        }


        public string this[string columnName]
        {
            get
            {
                if (columnName == "Location" && string.IsNullOrEmpty(Location))
                {
                    return "Number is not greater than 10!";
                }


                return null;
            }
        }


        private Station CreateCopy(Station model)
        {
            var copy = new Station
            {
                StationId = model.StationId,
                DisplayLocation = model.DisplayLocation,
                DisplayLocationArea = model.DisplayLocationArea,
                Imei = model.Imei,
                Keyword = model.Keyword,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Location = model.Location,
                PhoneNumber = model.PhoneNumber,
                
            };
            return copy;
        }
    }
}