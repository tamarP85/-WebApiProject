namespace WebApiProject.Models;
public class User:GenericModel
{ 
    public string Name { get; set;}
    public string Password { get; set;}
    public string Email { get; set;}
    public string Type { get; set;}
}