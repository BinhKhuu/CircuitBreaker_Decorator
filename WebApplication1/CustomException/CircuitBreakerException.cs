namespace WebApplication1.CustomException
{
    public class CircuitBreakerException : Exception
    {
        public CircuitBreakerException()
        {
        }

        public CircuitBreakerException(string message)
            : base(message)
        {
        }

        public CircuitBreakerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
