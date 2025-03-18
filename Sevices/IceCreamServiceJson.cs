
using WebApiProject.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using WebApiProject.Models;

namespace WebApiProject.Services;

    public class IceCreamServiceJson : IIceCreamService // שינוי: מימוש הממשק הגנרי
    {
        private List<IceCream> IceCreamList { get; }
        private static string fileName = "IceCream.json";
        private string filePath;

        public IceCreamServiceJson(IHostEnvironment env)
        {
            filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

            using (var jsonFile = File.OpenText(filePath))
            {
                IceCreamList = JsonSerializer.Deserialize<List<IceCream>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<IceCream>();
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(IceCreamList));
        }

        public List<IceCream> Get()
        {
            return IceCreamList;
        }

        public IceCream Get(int id) => IceCreamList.FirstOrDefault(i => i.Id == id);

        public int Insert(IceCream newIceCream)
        {
            if (newIceCream == null || newIceCream.Price <= 0 || string.IsNullOrWhiteSpace(newIceCream.Name))
                return -1;

            int maxId = IceCreamList.Max(i => i.Id);
            newIceCream.Id = maxId + 1;
            IceCreamList.Add(newIceCream);
            saveToFile();
            return newIceCream.Id;
        }

        public bool UpDate(int id, IceCream newIceCream)
        {
            if (newIceCream == null || newIceCream.Price <= 0 || string.IsNullOrWhiteSpace(newIceCream.Name) || newIceCream.Id != id)
                return false;

            var iceCream = IceCreamList.FirstOrDefault(i => i.Id == id);
            if (iceCream == null)
                return false;

            iceCream.Name = newIceCream.Name;
            iceCream.Price = newIceCream.Price; // שינוי: עדכון המחיר
            saveToFile();
            return true;
        }

        public bool Delete(int id)
        {
            var iceCream = IceCreamList.FirstOrDefault(i => i.Id == id);
            if (iceCream == null)
                return false;

            IceCreamList.Remove(iceCream);
            saveToFile();
            return true;
        }
    }
public static class IceCreamUtilitiesJson
{
    public static void AddIceCreamJson(this IServiceCollection services)
    {
        services.AddSingleton<IIceCreamService, IceCreamServiceJson>(); // שינוי: הוספת ה-Service כמימוש של הממשק הגנרי
    }
}
