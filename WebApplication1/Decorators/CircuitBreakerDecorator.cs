using WebApplication1.Interfaces;
using WebApplication1.Shared;

namespace WebApplication1.Decorators
{
    public class CircuitBreakerDecorator : IWeatherService
    {
        private IWeatherService _weatherService;
        private CircuitBreaker _circuitBreaker;
        public CircuitBreakerDecorator(IWeatherService weatherService) { 
            _weatherService = weatherService;
            _circuitBreaker = new CircuitBreaker();
        }
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            try
            {
                _circuitBreaker.CheckIfCircuitBreakerTimeStampIsComplete();
                if (!_circuitBreaker.isCircuitOpen)
                {
                    var forcasts = _weatherService.GetWeatherForecast();
                    // success reset circuit possible concurrency issues.
                    _circuitBreaker.RecordCircuitBreakerEnd();
                    return forcasts;
                }
                return null;
            }
            catch (Exception ex) 
            {
                if (!_circuitBreaker.isCircuitOpen && _circuitBreaker.attemptCount < _circuitBreaker.maxAttemptCount)
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
