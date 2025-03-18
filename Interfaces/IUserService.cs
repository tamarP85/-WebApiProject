using WebApiProject.Models;
namespace WebApiProject.Interfaces;
public interface IUserService
{
    List<User> Get();
    User Get(string email);
    int Insert(User newUser);
    bool UpDate(string email,User newUser);
    bool Delete(string email);
}