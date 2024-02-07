using FortressConquestApi.Common;

namespace FortressConquestApi.Models
{
    public class GeoLocation
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double LatitudeRadians { get; private set; }
        public double LongitudeRadians { get; private set; }

        private static double MinLatitude = Utils.DegreesToRadians(-90d);
        private static double MaxLatitude = Utils.DegreesToRadians(90d);
        private static double MinLongitude = Utils.DegreesToRadians(-180d);
        private static double MaxLongitude = Utils.DegreesToRadians(180d);

        private const double EarthRadiusInM = 6376500.0;

        private GeoLocation()
        {
        }

        public GeoLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;

            LatitudeRadians = Utils.DegreesToRadians(latitude);
            LongitudeRadians = Utils.DegreesToRadians(longitude);

            CheckBounds();
        }

        public static GeoLocation FromRadians(double latitude, double longitude)
        {
            var location = new GeoLocation
            {
                Latitude = Utils.RadiansToDegrees(latitude),
                Longitude = Utils.RadiansToDegrees(longitude),
                LatitudeRadians = latitude,
                LongitudeRadians = longitude
            };

            location.CheckBounds();

            return location;
        }

        public static GeoLocation FromDegrees(double latitude, double longitude)
        {
            return new GeoLocation(latitude, longitude);
        }

        private void CheckBounds()
        {
            if (LatitudeRadians < MinLatitude || LatitudeRadians > MaxLatitude)
            {
                throw new ArgumentOutOfRangeException(nameof(Latitude), "Latitude must be between -90 and 90 degrees.");
            }

            if (LongitudeRadians < MinLongitude || LongitudeRadians > MaxLongitude)
            {
                throw new ArgumentOutOfRangeException(nameof(Longitude), "Longitude must be between -180 and 180 degrees.");
            }
        }

        public double DistanceInMeters(GeoLocation otherLocation)
        {
            return Math.Acos(Math.Sin(LatitudeRadians) * Math.Sin(otherLocation.LatitudeRadians) +
                    Math.Cos(LatitudeRadians) * Math.Cos(otherLocation.LatitudeRadians) *
                    Math.Cos(LongitudeRadians - otherLocation.LongitudeRadians)) * EarthRadiusInM;
        }

        public (GeoLocation, GeoLocation) BoundingCoordinates(double radiusInM)
        {
            if (radiusInM < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radiusInM), "Radius must be a positive number.");
            }

            double radDist = radiusInM / EarthRadiusInM;

            double minLat = LatitudeRadians - radDist;
            double maxLat = LatitudeRadians + radDist;

            double minLon, maxLon;
            if (minLat > MinLatitude && maxLat < MaxLatitude)
            {
                double deltaLon = Math.Asin(Math.Sin(radDist) / Math.Cos(LatitudeRadians));

                minLon = LongitudeRadians - deltaLon;
                if (minLon < MinLongitude)
                {
                    minLon += 2d * Math.PI;
                }

                maxLon = LongitudeRadians + deltaLon;
                if (maxLon > MaxLongitude)
                {
                    maxLon -= 2d * Math.PI;
                }
            }
            else
            {
                minLat = Math.Max(minLat, MinLatitude);
                maxLat = Math.Min(maxLat, MaxLatitude);
                minLon = MinLongitude;
                maxLon = MaxLongitude;
            }   

            return (FromRadians(minLat, minLon), FromRadians(maxLat, maxLon));
        }
    }
}
