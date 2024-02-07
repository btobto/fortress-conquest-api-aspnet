namespace FortressConquestApi.Common
{
    public static class Utils
    {
        public static  double DegreesToRadians(double x)
        {
            return x * (Math.PI / 180);
        }

        public static double RadiansToDegrees(double x)
        {
            return x * (180 / Math.PI);
        }
    }
}
