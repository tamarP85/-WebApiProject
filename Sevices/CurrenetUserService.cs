using WebApiProject.Models;

namespace WebApiProject.Services;

public class CurrentUserService{
    public static CurrentUser currentUser;
    public CurrentUserService(string type,int id){
        if(currentUser==null)
            currentUser=new CurrentUser(type,id);
        // else
        //     currentUser=currentUser;

    }
}