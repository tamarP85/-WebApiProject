using WebApiProject.Models;
namespace WebApiProject.Interfaces;
public interface IGenericInterface<T>
{
    List<T> Get();
    T Get(int id);
    int Insert(T newItem);
    bool UpDate(int id,T newItem);
    bool Delete(int id);
}