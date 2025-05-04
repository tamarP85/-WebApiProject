using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;
using Microsoft.AspNetCore.Authorization;
namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class IceCreamController : ControllerBase
{

    private IGenericServicesJson<IceCream> iceCreamService;

    public IceCreamController(IGenericServicesJson<IceCream> iceCreamService)
    {
        this.iceCreamService = iceCreamService;
    }
    [HttpGet]
    [Authorize(Policy = "Agent")]
    public ActionResult<IEnumerable<IceCream>> Get()
    {
        System.Console.WriteLine("I am in controller %%%%%%%%%%%%%%%%%%%%%");
        System.Console.WriteLine("1111111111111"+iceCreamService.Get(12));
        var derivedService = iceCreamService as IceCreamServiceJson;
        var a = derivedService.Get();
        System.Console.WriteLine("==================="+derivedService);
        System.Console.WriteLine("8888888888888888888888");
        System.Console.WriteLine(a);
        return a;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Agent")]
    public ActionResult<IceCream> Get(int id)
    {
        var iceCream = iceCreamService.Get(id);
        if (iceCream == null)
            return NotFound();
        return iceCream;
    }

    [HttpPost]
    [Authorize(Policy = "Agent")]
    public ActionResult Post(IceCream newIceCream)
    {
        var newId = iceCreamService.Insert(newIceCream);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Agent")]
    public ActionResult Put(int id, IceCream newIceCream)
    {
        if (iceCreamService.UpDate(id, newIceCream))
            return NoContent();
        return BadRequest();
    }
    [HttpDelete("{id}")]
    [Authorize(Policy = "Agent")]
    public ActionResult Delete(int id)
    {
        System.Console.WriteLine(id);
        if (iceCreamService.Delete(id))
            return Ok();

        return NotFound();
    }

}
