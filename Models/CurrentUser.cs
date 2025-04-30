namespace WebApiProject.Models;
public class CurrentUser:GenericModel{
    public string Type { get; set;}
public CurrentUser(string Type,int id){
    this.Id=id;
    this.Type=Type;
}
}