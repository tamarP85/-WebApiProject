using System.Net;
using System.Net.Mail;

namespace WebApiProject.Middleware;

public class ErrorMiddleware{


    private RequestDelegate next;

    public ErrorMiddleware( RequestDelegate next){
        this.next=next;
    }

    public async Task Invoke(HttpContext context ){
     context.Items["success"]=false;
     bool success=false;
     try
     {
        await next(context);
        context.Items["success"]=true;
     }
     catch (ApplicationException ex)
     {
        context.Response.StatusCode=400;
        await   context.Response.WriteAsync(ex.Message);
     }
     catch (Exception e)
     {
        context.Response.StatusCode=500;
        MailMessage mail =new MailMessage("y05271907@gmail.com","9745544b@gmail.com",$"תקלה בשרת {e.Message}","פנה לתמיכה התכנית");
        SmtpClient stmp = new SmtpClient("stnp.gmail.com",587){
            Credentials=new NetworkCredential("y05271907@gmail.com","y089741623"),
            EnableSsl=true
        };
        try{
            stmp.Send(mail);
            Console.WriteLine("המייל נשלח בהצלחה");
        }
        catch(Exception ex){
            Console.WriteLine("שגיאה בשליחת המייל"+ex.Message);
        }
     }
    }
}
public static partial class MiddlewareExtentions{
    public static WebApplication UseErrorMiddleware(this WebApplication app){
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }
}
