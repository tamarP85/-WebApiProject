using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers;
using WebApiProject.Models;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly List<WeatherForecast> listDays;

    public WeatherForecastController()
    {
        listDays = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToList();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return listDays;
    }

    [HttpGet("{id}")]
    public ActionResult<WeatherForecast> Get(int id)
    {
        if (id < 0 || id > 5)
            return BadRequest("not valid");
        return listDays[id];
    }

    [HttpPost]
    public void Post(WeatherForecast newItem)
    {
        listDays.Add(newItem);
    }

    [HttpPut]
    public ActionResult<WeatherForecast> Put(int id, WeatherForecast newItem)
    {
        if (id < 0 || id > 5)
            return BadRequest("not valid");
        listDays[id] = newItem;
        return NoContent();
    }

}
