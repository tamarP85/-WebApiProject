using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserService UserService;
    public UserController(IUserService userService)
    {
        this.UserService = userService;
    } 
    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        return UserService.Get();
    }

    [HttpGet("{email}")]
    public ActionResult<User> Get(string email)
    {
        var User = UserService.Get(email);
        if (User == null)
            return NotFound();
        return User;
    }

    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = UserService.Insert(newUser);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{email}")]
    public ActionResult Put(string email, User newUser)
    {
        if (UserService.UpDate(email, newUser))
            return NoContent();
        return BadRequest();
    }
    [HttpDelete("{email}")]
    public ActionResult Delete(string email)
    {
        if (UserService.Delete(email))
            return Ok();
        return NotFound();
    }
}
