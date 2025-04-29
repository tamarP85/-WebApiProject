using WebApiProject.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using WebApiProject.Models;

namespace WebApiProject.Services;
public abstract class IGenericServicesJson<T> : IGenericInterface<T> where T : GenericModel
{
    protected List<T> ItemsList { get; }
    protected string fileName;

    protected string filePath;

    // public IGenericServicesJson(IHostEnvironment env)
    // {
    //     fileName = typeof(T).Name + ".json";
    //     filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
    //     System.Console.WriteLine("-----------------");

    //     System.Console.WriteLine(filePath);
    //     using (var jsonFile = File.OpenText(filePath))
    //     {
    //         ItemsList = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
    //         new JsonSerializerOptions
    //         {
    //             PropertyNameCaseInsensitive = true
    //         }) ?? new List<T>();
    //     }
    // }
    public IGenericServicesJson(IHostEnvironment env)
    {
        fileName = typeof(T).Name + ".json"; // הגדרה ברמת האינסטנציה
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        System.Console.WriteLine("-----------------");
        System.Console.WriteLine(filePath);
        System.Console.WriteLine($"Current Type: {typeof(T).Name}");
        System.Console.WriteLine($"File Name: {fileName}");
        System.Console.WriteLine($"File Path: {filePath}");
        using (var jsonFile = File.OpenText(filePath))
        {
            ItemsList = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<T>();
        }
    }
    protected void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(ItemsList));
    }

    public List<T> Get()
    {
        return ItemsList;
    }

    public T Get(int id) => ItemsList.FirstOrDefault(i => i.Id == id);


    public abstract int Insert(T newItem);


    public abstract bool UpDate(int id, T newItem);
    public bool Delete(int id)
    {
        var item = ItemsList.FirstOrDefault(i => i.Id == id);
        if (item == null)
            return false;

        ItemsList.Remove(item);
        saveToFile();
        return true;
    }
}

public static class GenericUtilitiesJson
{
    public static void AddGenericJson<T, TService>(this IServiceCollection services)
       where T : GenericModel
       where TService : IGenericServicesJson<T>
    {
        services.AddScoped<IGenericServicesJson<T>, TService>();
    }
}
