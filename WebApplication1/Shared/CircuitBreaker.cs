namespace WebApplication1.Shared
{
    public class CircuitBreaker
    {
        //count of failed attempts that have been made
        public int attemptCount = 0;

        //count of failed attempts allowed, post which the circiut will break
        public int maxAttemptCount = 3;

        //flag to represent if the circuit is open or close
        public bool isCircuitOpen = false;

        //field to keep a track of the utc time when the circuit was opened/broken
        public DateTime circuitOpenStartTime = DateTime.MinValue;

        //the timestamp (in millisecond) for which the circuit should remain open and api call attempts should not be made
        public double circuitBreakerTimeSpanMilliseconds = TimeSpan.FromSeconds(10).TotalMilliseconds;

        //method to start the circuit breaker
        //if sets the isCircuitOpen to true and sets the time when the circuit was broken in utc
        public void RecordCircuitBreakerStart()
        {
            circuitOpenStartTime = DateTime.UtcNow;
            isCircuitOpen = true;
        }

        //method to end the circuit breaker
        public void RecordCircuitBreakerEnd()
        {
            circuitOpenStartTime = DateTime.MinValue;
            isCircuitOpen = false;
            attemptCount = 0;
        }

        //check if currently the circuit is broken or not
        public void CheckIfCircuitBreakerTimeStampIsComplete()
        {
            if (isCircuitOpen == true && circuitOpenStartTime.AddMilliseconds(circuitBreakerTimeSpanMilliseconds) < DateTime.UtcNow)
            {
                RecordCircuitBreakerEnd();
            }
        }
    }
}
