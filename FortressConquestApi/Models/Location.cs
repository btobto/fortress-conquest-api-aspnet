using FortressConquestApi.Common;
using Newtonsoft.Json.Linq;

namespace FortressConquestApi.Models
{
    public class Location
    {
        public long Latitude { get; private set; }
        public long Longitude { get; private set; }

        public Location(long latitude, long longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(Latitude), "Latitude must be between -90 and 90 degrees.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(Longitude), "Longitude must be between -180 and 180 degrees.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public double DistanceInMeters(Location otherLocation)
        {
            var d1 = Utils.ToRadians(Latitude);
            var num1 = Utils.ToRadians(Longitude);
            var d2 = Utils.ToRadians(otherLocation.Latitude);
            var num2 = Utils.ToRadians(otherLocation.Longitude) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

        }
    }
}
