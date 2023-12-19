namespace WebApplication1.Interfaces
{
    public interface IWeatherService
    {
        public IEnumerable<WeatherForecast> GetWeatherForecast();
    }
}
