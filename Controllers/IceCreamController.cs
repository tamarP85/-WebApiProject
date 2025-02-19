using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class IceCreamController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<IceCream>> Get()
    {
        return IceCreamService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<IceCream> Get(int id)
    {
        var iceCream = IceCreamService.Get(id);
        if (iceCream == null)
            return NotFound();
        return iceCream;
    }

    [HttpPost]
    public ActionResult Post(IceCream newIceCream)
    {
        var newId = IceCreamService.Insert(newIceCream);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, IceCream newIceCream)
    {
        if (IceCreamService.UpDate(id, newIceCream))
            return NoContent();
        return BadRequest();
    }
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (IceCreamService.Delete(id))
            return Ok();
        return NotFound();
    }
}
