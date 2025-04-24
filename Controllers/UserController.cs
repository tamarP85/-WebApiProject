using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : GenericController<User>
{
    private IGenericInterface<User> UserService;
    public UserController(IGenericServicesJson<User> userService):base(userService)
    {
    } 
}
