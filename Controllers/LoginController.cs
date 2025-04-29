using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebApiProject.Models;
using WebApiProject.Services;
namespace WebApiProject.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        public LoginController() { }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            var dt = DateTime.Now;
              Console.WriteLine("++++++++++++++++++++");
  Console.WriteLine(User.Name);
            if (User.Name != "Wray")
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
                new Claim("ClearanceLevel", "2"),
            };

            var token = TokenService.GetToken(claims);
            Console.WriteLine(token);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
    }
