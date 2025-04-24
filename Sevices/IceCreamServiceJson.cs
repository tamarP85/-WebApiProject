
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

        public IceCreamServiceJson(IHostEnvironment env):base(env)
        {
        }
       
        public override int Insert(IceCream newIceCream)
        {
            if (newIceCream == null || newIceCream.Price <= 0 || string.IsNullOrWhiteSpace(newIceCream.Name))
                return -1;

            int maxId = ItemsList.Max(i => i.Id);
            newIceCream.Id = maxId + 1;
            ItemsList.Add(newIceCream);
            saveToFile();
            return newIceCream.Id;
        }

        public  override bool UpDate(int id, IceCream newIceCream)
        {
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
