namespace FortressConquestApi.Common.Exceptions
{
    public class EmailTakenException : Exception
    {
        public EmailTakenException()
        {
        }

        public EmailTakenException(string? message) : base(message)
        {
        }

        public EmailTakenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
