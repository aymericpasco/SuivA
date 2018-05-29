using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using SuivA.Data.Entity;
using SuivA.Data.Utility;

namespace SuivA.DesktopClient.Service
{
    public class GpsService
    {
        public static GpsUtility.Location GetCoordinates(Office office)
        {
            if (office == null) return new GpsUtility.Location();
            var address = Convert.ToString(office.StreetNumber) + " " + office.StreetName + " " +
                          Convert.ToString(office.ZipCode) + " " + office.City;

            var request = WebRequest
                .Create("https://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address="
                        + HttpUtility.UrlEncode(address) + "&key=AIzaSyAMW5u-VCR_8-zB5MOgZY5sJuo7Q1WZiSQ");

            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var document = XDocument.Load(new StreamReader(stream ?? throw new InvalidOperationException()));

                    var longitudeElement = document.Descendants("lng").FirstOrDefault();
                    var latitudeElement = document.Descendants("lat").FirstOrDefault();

                    if (longitudeElement != null && latitudeElement != null)
                        return new GpsUtility.Location
                        {
                            Longitude = double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture),
                            Latitude = double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture)
                        };
                }
            }

            return new GpsUtility.Location();
        }
    }
}