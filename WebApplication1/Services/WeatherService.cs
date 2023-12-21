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
            throw new Exception(); //simulate error
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
