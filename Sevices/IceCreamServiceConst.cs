using WebApiProject.Interfaces;
using WebApiProject.Models;
namespace WebApiProject.Services;
public class IceCreamServiceConst:IIceCreamService
{
    private  List<IceCream> iceCreamList;
    public IceCreamServiceConst()
    {
        iceCreamList = new List<IceCream>
        {
            new IceCream{
                 Id  =  1 ,
                 Name  = " שוקולד בלגי" ,
                 Price  =  15 ,
                 Description   = " חויה שוקולדית מפנקת בטעם של עוד" ,
            },
            new IceCream{
                 Id  =  9 ,
                 Name  = " וניל-שוקולד-צ׳יפס" ,
                 Price  =  16 ,
                 Description  = " גלידה מפנקת בטעם של עוד" ,
            },
            new IceCream{
                 Id  =  3 ,
                 Name  = " שוקולד בייגלה" ,
                 Price  =  12 ,
                 Description    = " גלידה מפנקת בטעם של עוד" ,
            }
        };
    }
    public List<IceCream> Get()
    {
        return iceCreamList;
    }
    public  IceCream Get(int id)
    {
        var iceCream = iceCreamList.FirstOrDefault(i=>i.Id == id);
        return iceCream;
    }
    public  int Insert(IceCream newIceCream)
    {
        if(newIceCream == null
            || newIceCream.Price <= 0
            || string.IsNullOrWhiteSpace(newIceCream.Name))
        return -1;
        int maxId = iceCreamList.Max(i=>i.Id);
        newIceCream.Id=maxId+1;
        iceCreamList.Add(newIceCream);
        return newIceCream.Id;
    }
    public bool UpDate(int id,IceCream newIceCream)
    {
        if(newIceCream == null
            || newIceCream.Price <= 0
            || string.IsNullOrWhiteSpace(newIceCream.Name)
            || newIceCream.Id != id)
        return false;
        var iceCream = iceCreamList.FirstOrDefault(i => i.Id == id);
        if(iceCream == null)
            return false;
        iceCream.Name = newIceCream.Name;
        iceCream.Id = newIceCream.Id;
        return true;
    }
    public bool Delete(int id){
        var iceCream = iceCreamList.FirstOrDefault(i => i.Id == id);
        if(iceCream == null)
            return false;
        var index = iceCreamList.IndexOf(iceCream);
        iceCreamList.RemoveAt(index);
        return true;
    }

}
public static class IceCreamUtilities
{
    public static void AddIceCreamConst(this IServiceCollection services){
        services.AddSingleton<IIceCreamService,IceCreamServiceConst>();
    }
}