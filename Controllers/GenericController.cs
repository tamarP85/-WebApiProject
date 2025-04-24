using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class GenericController<T> : ControllerBase where T:GenericModel
{
    private IGenericServicesJson<T> service;
    public GenericController(IGenericServicesJson<T> service)
    {
        this.service = service;
    } 
    [HttpGet]
    public ActionResult<IEnumerable<T>> Get()
    {
        return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<T> Get(int id)
    {
        var item = service.Get(id);
        if (item == null)
            return NotFound();
        return item;
    }

    [HttpPost]
    public ActionResult Post(T newItem)
    {
        var newId = service.Insert(newItem);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, T newItem)
    {
        if (service.UpDate(id, newItem))
            return NoContent();
        return BadRequest();
    }
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (service.Delete(id))
            return Ok();
        return NotFound();
    }
}
