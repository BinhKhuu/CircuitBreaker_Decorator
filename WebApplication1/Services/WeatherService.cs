using System.ComponentModel;
using WebApplication1.CustomException;
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
        public WeatherService() 
        { 
        }

        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            //while (true)
            //    throw new exception();

            while (true)
                throw new CircuitBreakerException();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
