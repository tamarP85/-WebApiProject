using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
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
public ActionResult<String> Login([FromBody] User user)
{
    User currentUser = userService.Get().FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
    if (currentUser == null)
    {
        return NotFound(); 
    }

    var claims = new List<Claim>
    {
        new Claim("Id", currentUser.Id.ToString()),
        new Claim("type", currentUser.Type) 
    };

    var claimsIdentity = new ClaimsIdentity(claims, "login");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    HttpContext.SignInAsync(claimsPrincipal);

    var token = TokenService.GetToken(claims);
    return new OkObjectResult(TokenService.WriteToken(token));
}

}
