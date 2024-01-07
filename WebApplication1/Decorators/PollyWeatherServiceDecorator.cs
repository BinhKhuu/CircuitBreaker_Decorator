using Polly.CircuitBreaker;
using Polly.Retry;
using Polly;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using System.Net;
using WebApplication1.CustomException;

namespace WebApplication1.Decorators
{
    public class PollyWeatherServiceDecorator : IWeatherService
    {
        private readonly IWeatherService _innerService;
        public PollyWeatherServiceDecorator(IWeatherService serivce) 
        {
            _innerService = serivce;
        }

        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            //var optionsShouldHandle = new CircuitBreakerStrategyOptions<HttpResponseMessage>
            //{
            //    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            //        .Handle<CircuitBreakerException>()
            //        .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError)
            //};

            var optionsComplex = new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(30),
                ShouldHandle = new PredicateBuilder().Handle<CircuitBreakerException>()
            };

            ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions()) // Add retry using the default options
                .AddCircuitBreaker(optionsComplex)
                .AddTimeout(TimeSpan.FromSeconds(10)) // Add 10 seconds timeout
                .Build(); // Builds the resilience pipeline

            var stateProvider = new CircuitBreakerStateProvider();
            var optionsStateProvider = new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                StateProvider = stateProvider
            };

            var circuitState = stateProvider.CircuitState;

            return pipeline.Execute(token => _innerService.GetWeatherForecast());
        }
    }
}
