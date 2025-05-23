
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

public class UserServiceJson : IGenericServicesJson<User>
{
    private readonly IHostEnvironment _env; 
    public IceCreamServiceJson iceCreamService;

    public ActiveUserService activeUserService;
    public UserServiceJson(IHostEnvironment env, IceCreamServiceJson iceCreamService,ActiveUserService activeUserService) : base(env)
    {
        _env = env;
        this.iceCreamService = iceCreamService;
        activeUserService=activeUserService;
    }

    public User Get(int id)
    {
        if (activeUserService.UserId==id||activeUserService.Type=="Admin")
            return ItemsList.FirstOrDefault(i => i.Id == id);
        else
            return null;
    }
    public override int Insert(User newUser)
    {
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Password) || string.IsNullOrWhiteSpace(newUser.Type)||(newUser.Type!="Admin"&&newUser.Type!="Agent")
        )
            return -1;
        if (!IsValidEmail(newUser.Email))
            return -1;

        int maxId = ItemsList.Max(i => i.Id);
        newUser.Id = maxId + 1;
        if (ItemsList.FirstOrDefault(u => u.Email == newUser.Email || u.Password == newUser.Password) != null)
            return -1;
        ItemsList.Add(newUser);
        saveToFile();
        return newUser.Id;
    }

    public override bool UpDate(int id, User newUser)
    {
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Password) || string.IsNullOrWhiteSpace(newUser.Type)||(newUser.Type!="Admin"&&newUser.Type!="Agent"))
            return false;
        var User = ItemsList.FirstOrDefault(i => i.Id == id);
        if (User == null)
            return false;
        if (ItemsList.FirstOrDefault(u => (u.Email == newUser.Email && u.Id != User.Id) || (u.Password == newUser.Password && u.Id != User.Id)) != null)
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
    public override bool Delete(int id)
    {
        var item = ItemsList.FirstOrDefault(i => i.Id == id);
        if (item == null)
            return false;        
        List<IceCream> iceCreamsToDelete = iceCreamService.Get().Where(i => i.AgentId == id).ToList();
        foreach (var iceCream in iceCreamsToDelete)
        {
            
            iceCreamService.Delete(iceCream.Id);
        }
        ItemsList.Remove(item);
        saveToFile();
        return true;
    }
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}

