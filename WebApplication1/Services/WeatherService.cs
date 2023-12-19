using WebApplication1.Interfaces;
using WebApplication1.Shared;

namespace WebApplication1.Services
{
    public class WeatherService : IWeatherService
    {

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private CircuitBreaker _circuitBreaker;
        public WeatherService() 
        { 
            _circuitBreaker = new CircuitBreaker();
        }

        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            try
            {
                _circuitBreaker.CheckIfCircuitBreakerTimeStampIsComplete();

                if (!_circuitBreaker.isCircuitOpen)
                {
                    throw new Exception(); //simulate error
                    var forcasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray();

                    // success reset circuit possible concurrency issues.
                    _circuitBreaker.RecordCircuitBreakerEnd();
                    return forcasts;
                }

                return null;

            }
            catch (Exception ex) 
            {
                // increment threshold
                if(!_circuitBreaker.isCircuitOpen && _circuitBreaker.attemptCount < _circuitBreaker.maxAttemptCount) 
                {
                    _circuitBreaker.attemptCount++;
                }
                // check if circuit has been tripped
                if (_circuitBreaker.attemptCount == _circuitBreaker.maxAttemptCount)
                {
                    if (_circuitBreaker.isCircuitOpen == false)
                    {
                        _circuitBreaker.RecordCircuitBreakerStart();
                    }
                    return null;
                }

                throw ex;
            }


        }
    }
}
