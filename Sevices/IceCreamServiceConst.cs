using WebApiProject.Interfaces;
using WebApiProject.Models;
namespace WebApiProject.Services;
public class IceCreamServiceConst:IGenericInterface<IceCream>
{
    private  List<IceCream> IceCreamList;
    public IceCreamServiceConst()
    {
        IceCreamList = new List<IceCream>
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
        return IceCreamList;
    }
    public  IceCream Get(int id)
    {
        var iceCream = IceCreamList.FirstOrDefault(i=>i.Id == id);
        return iceCream;
    }
    public  int Insert(IceCream newIceCream)
    {
        if(newIceCream == null
            || newIceCream.Price <= 0
            || string.IsNullOrWhiteSpace(newIceCream.Name))
        return -1;
        int maxId = IceCreamList.Max(i=>i.Id);
        newIceCream.Id=maxId+1;
        IceCreamList.Add(newIceCream);
        return newIceCream.Id;
    }
    public bool UpDate(int id,IceCream newIceCream)
    {
        if(newIceCream == null
            || newIceCream.Price <= 0
            || string.IsNullOrWhiteSpace(newIceCream.Name)
            || newIceCream.Id != id)
        return false;
        var iceCream = IceCreamList.FirstOrDefault(i => i.Id == id);
        if(iceCream == null)
            return false;
        iceCream.Name = newIceCream.Name;
        iceCream.Id = newIceCream.Id;
        return true;
    }
    public bool Delete(int id){
        var iceCream = IceCreamList.FirstOrDefault(i => i.Id == id);
        if(iceCream == null)
            return false;
        var index = IceCreamList.IndexOf(iceCream);
        IceCreamList.RemoveAt(index);
        return true;
    }

}
public static class IceCreamUtilities
{
    public static void AddIceCreamConst(this IServiceCollection services){
        services.AddSingleton<IGenericInterface<IceCream>,IceCreamServiceConst>();
    }
}