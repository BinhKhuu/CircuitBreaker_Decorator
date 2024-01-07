using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly.Retry;
using Polly;
using WebApplication1.Interfaces;
using System.Threading;
using Polly.CircuitBreaker;
using WebApplication1.Services;
using WebApplication1.Decorators;
using WebApplication1.CustomException;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollyWeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public PollyWeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetPollyWeatherForecast")]
        async public Task<IActionResult> Get()
        {
            try
            {
                var weatherService = new PollyWeatherServiceDecorator(new WeatherService());
                var request = weatherService.GetWeatherForecast();
                if (request == null)
                {
                    // returns 400 error when circuit is open
                    
                }
                return Ok(request.ToList());
            }
            catch (CircuitBreakerException)
            {
                return BadRequest("Service is down");
            }
            catch (Exception ex)
            {
                // returns 500 error 
                throw ex;
            }

        }
    }
}
