using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult  Get()
        {
            try
            {
                var request = _weatherService.GetWeatherForecast();
                if (request == null)
                {
                    // returns 400 error when circuit is open
                    return BadRequest("Service is down");
                }
                return Ok(request.ToList());
            }
            catch (Exception ex)
            {
                // returns 500 error 
                throw ex;
            }
            
        }
    }
}