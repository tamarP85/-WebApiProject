using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class IceCreamController : ControllerBase
{
    private IIceCreamService iceCreamService;
    public IceCreamController(IIceCreamService iceCreamService)
    {
        this.iceCreamService = iceCreamService;
    } 
    [HttpGet]
    public ActionResult<IEnumerable<IceCream>> Get()
    {
        throw new Exception();
        return iceCreamService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<IceCream> Get(int id)
    {
        var iceCream = iceCreamService.Get(id);
        if (iceCream == null)
            return NotFound();
        return iceCream;
    }

    [HttpPost]
    public ActionResult Post(IceCream newIceCream)
    {
        var newId = iceCreamService.Insert(newIceCream);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, IceCream newIceCream)
    {
        if (iceCreamService.UpDate(id, newIceCream))
            return NoContent();
        return BadRequest();
    }
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (iceCreamService.Delete(id))
            return Ok();
        return NotFound();
    }
}
