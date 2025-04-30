using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;
namespace WebApiProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private IGenericServicesJson<User> userService;
    public LoginController(IGenericServicesJson<User> userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] User user)
    {
        User currentUser = userService.Get().FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
        System.Console.WriteLine("----------------");
        System.Console.WriteLine(currentUser);
        if (currentUser == null)
        {
            return NotFound(); 
        }

        var claims = new List<Claim>();
        if (currentUser.Type == "Agent")
        {

            claims = new List<Claim>
           {
                new Claim("type", "Agent"),
            };
        }
        else
        {
            claims = new List<Claim>
            {
                new Claim("type", "Admin"),
            };
        }


        var token = TokenService.GetToken(claims);
        CurrentUserService current=new CurrentUserService(currentUser.Type,currentUser.Id);
        Console.WriteLine(current.ToString());
        return new OkObjectResult(TokenService.WriteToken(token));
    }
}
