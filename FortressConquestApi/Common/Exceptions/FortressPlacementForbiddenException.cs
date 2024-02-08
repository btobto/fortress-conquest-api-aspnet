namespace FortressConquestApi.Common.Exceptions
{
    public class FortressPlacementForbiddenException : Exception
    {
        public FortressPlacementForbiddenException()
        {
        }

        public FortressPlacementForbiddenException(string? message) : base(message)
        {
        }

        public FortressPlacementForbiddenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}