using WebApiProject.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using WebApiProject.Models;

namespace WebApiProject.Services;

public class IceCreamServiceJson : IGenericServicesJson<IceCream> // שינוי: מימוש הממשק הגנרי
{

    public IceCreamServiceJson(IHostEnvironment env) : base(env)
    {
    }
    public new List<IceCream> Get()
    {
        //System.Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        System.Console.WriteLine(ItemsList.Where(c => c.AgentId == CurrentUserService.currentUser.Id).ToList());
        System.Console.WriteLine("i am in ovveride");
        if (CurrentUserService.currentUser.Type == "Admin")
            return ItemsList;
        return ItemsList.Where(c => c.AgentId == CurrentUserService.currentUser.Id).ToList();
    }

    public IceCream Get(int id)
    {
        if (CheckTokenService.isValidRequest(id) != -1)
            return ItemsList.FirstOrDefault(i => i.Id == id);
        else
            return null;
    }
    public override bool Delete(int id)
    {
        var item = ItemsList.FirstOrDefault(i => i.Id == id);

        if (item == null)
            return false;
        if (CheckTokenService.isValidRequest(item.AgentId) != -1)
        {
            ItemsList.Remove(item);
            saveToFile();
            return true;
        }
        return false;
    }


    public override int Insert(IceCream newIceCream)
    {
        if (newIceCream == null || newIceCream.Price <= 0 || string.IsNullOrWhiteSpace(newIceCream.Name))
            return -1;
        newIceCream.AgentId = CurrentUserService.currentUser.Id;
        int maxId = ItemsList.Max(i => i.Id);
        newIceCream.Id = maxId + 1;
        ItemsList.Add(newIceCream);
        saveToFile();
        return newIceCream.Id;
    }

    public override bool UpDate(int id, IceCream newIceCream)
    {
        if (CheckTokenService.isValidRequest(id) != -1)
            return false;
        if (newIceCream == null || newIceCream.Price <= 0 || string.IsNullOrWhiteSpace(newIceCream.Name) || newIceCream.Id != id)
            return false;

        var iceCream = ItemsList.FirstOrDefault(i => i.Id == id);
        if (iceCream == null)
            return false;

        iceCream.Name = newIceCream.Name;
        iceCream.Price = newIceCream.Price; // שינוי: עדכון המחיר
        saveToFile();
        return true;
    }

}
