using WebApiProject.Models;
namespace WebApiProject.Interfaces;
public interface IIceCreamService
{
    List<IceCream> Get();
    IceCream Get(int id);
    int Insert(IceCream newIceCream);
    bool UpDate(int id,IceCream newIceCream);
    bool Delete(int id);
}