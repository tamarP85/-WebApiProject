using WebApiProject.Models;

namespace WebApiProject.Services;

public class CheckTokenService
{
    public static int isValidRequest(int currentId)
    {
        int userId = CurrentUserService.currentUser.Id;
        string userType = CurrentUserService.currentUser.Type;
        if (userId == -1)
            return -1;
        if (userType != "Admin" && userId != currentId)
            return -1;
        return userId;
    }
    // private static int decoder(string? token)
    // {
    //     if (string.IsNullOrEmpty(token))
    //     {
    //         Console.WriteLine("Token is null or empty");
    //         return -1;
    //     }
    //     if (token.StartsWith("Bearer "))
    //     {
    //         token = token.Substring(7);
    //     }

    //     var handler = new JwtSecurityTokenHandler();
    //     Console.WriteLine(handler.CanReadToken(token) ? "Token is readable" : "Token is NOT readable");

    //     try
    //     {
    //         if (!handler.CanReadToken(token))
    //         {
    //             Console.WriteLine("Invalid JWT token format");
    //             return -1;
    //         }

    //         var jwtToken = handler.ReadJwtToken(token);

    //         if (jwtToken.Payload.ContainsKey("id"))
    //         {
    //             return jwtToken.Payload["id"];
    //         }
    //         else
    //         {
    //             Console.WriteLine("Token does not contain 'id' claim.");
    //             return -1;
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error decoding token: {ex.Message}");
    //         return -1;
    //     }
    // }

}