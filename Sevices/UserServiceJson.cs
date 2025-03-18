
using WebApiProject.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using WebApiProject.Models;
using System.Text.RegularExpressions;
namespace WebApiProject.Services;

public class UserServiceJson : IUserService
{
    private List<User> UsersList { get; }
    private static string fileName = "Users.json";
    private string filePath;

    public UserServiceJson(IHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

        using (var jsonFile = File.OpenText(filePath))
        {
            UsersList = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<User>();
        }
    }

    private void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(UsersList));
    }

    public List<User> Get()
    {
        return UsersList;
    }

    public User Get(string email) => UsersList.FirstOrDefault(i => i.Email == email);

    public int Insert(User newUser)
    {
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Password) || string.IsNullOrWhiteSpace(newUser.Type))
            return -1;
        if (!IsValidEmail(newUser.Email))
            return -1;

        int maxId = UsersList.Max(i => i.Id);
        newUser.Id = maxId + 1;
        if (UsersList.FirstOrDefault(u => u.Email == newUser.Email || u.Password == newUser.Password) != null)
            return -1;
        UsersList.Add(newUser);
        saveToFile();
        return newUser.Id;
    }

    public bool UpDate(string email, User newUser)
    {
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Password) || string.IsNullOrWhiteSpace(newUser.Type))
            return false;
        var User = UsersList.FirstOrDefault(i => i.Email == email);
        if (User == null)
            return false;
        if (UsersList.FirstOrDefault(u => (u.Email == newUser.Email && u.Id != User.Id) || (u.Password == newUser.Password && u.Id != User.Id)) != null)
            return false;
        if (!IsValidEmail(newUser.Email))
            return false;
        User.Name = newUser.Name;
        User.Id = newUser.Id;
        User.Password = newUser.Password;
        User.Email = newUser.Email;
        User.Type = newUser.Type;
        saveToFile();
        return true;
    }

    public bool Delete(string email)
    {
        var User = UsersList.FirstOrDefault(i => i.Email == email);
        if (User == null)
            return false;

        UsersList.Remove(User);
        saveToFile();
        return true;
    }
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}
public static class UserUtilitiesJson
{
    public static void AddUserJson(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserServiceJson>();
    }
}
