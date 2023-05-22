using Microsoft.AspNetCore.Mvc;

namespace MvcLambda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // https://glr7pi9yub.execute-api.ap-southeast-1.amazonaws.com/Live/WeatherForecast

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // https://nnc29xgvo7.execute-api.ap-southeast-1.amazonaws.com/Live/WeatherForecast/add/5/6

        [HttpGet("add/{x}/{y}")]
        public IActionResult Add(int x, int y)
        {
            var str = string.Format($"Try to do {x} and {y}");
            return Ok(str);
        }
    }
}