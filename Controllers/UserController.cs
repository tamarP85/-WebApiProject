using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IGenericServicesJson<User> userService;
    public UserController(IGenericServicesJson<User> userService)
    {
        this.userService = userService;
    }
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<IEnumerable<User>> Get()
    {
        Console.WriteLine("************************");
        return userService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Agent")]
    public ActionResult<User> Get(int id)
    {
        var user = userService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Post(User newUser)
    {
        var newId = userService.Insert(newUser);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Put(int id, User newUser)
    {
        if (userService.UpDate(id, newUser))
            return NoContent();
        return BadRequest();
    }
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        if (userService.Delete(id))
            return Ok();
        return NotFound();
    }
}
