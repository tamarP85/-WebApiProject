using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers;
[ApiController]
[Route("[controller]")]
public class IceCreamController : GenericController<IceCream>
{
    private IGenericServicesJson<IceCream> iceCreamService;
    public IceCreamController(IGenericServicesJson<IceCream> iceCreamService):base(iceCreamService)
    {
    }
    } 
