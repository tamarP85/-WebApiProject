using WebApiProject.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using WebApiProject.Models;

namespace WebApiProject.Services;

    public class IceCreamServiceJson : IGenericServicesJson<IceCream> 
    {
        private readonly ActiveUserService activeUserService;

        public IceCreamServiceJson(IHostEnvironment env, ActiveUserService activeUserService) : base(env)
        {
            this.activeUserService = activeUserService;
        }

        public override List<IceCream> Get()
        {
            if (activeUserService.Type == "Admin")
                return ItemsList;
            return ItemsList.Where(c => c.AgentId == activeUserService.UserId).ToList();
        }

        public IceCream Get(int id)
        {
            if (isValidRequest(id))
                return ItemsList.FirstOrDefault(i => i.Id == id);
            return null;
        }

        public override bool Delete(int id)
        {
            if (!isValidRequest(id))
                return false;

            var item = ItemsList.FirstOrDefault(i => i.Id == id);
            if (item != null)
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

            newIceCream.AgentId = activeUserService.UserId;
            int maxId = ItemsList.Max(i => i.Id);
            newIceCream.Id = maxId + 1;
            ItemsList.Add(newIceCream);
            saveToFile();
            return newIceCream.Id;
        }

        public override bool UpDate(int id, IceCream newIceCream)
        {
            if (!isValidRequest(id))
                return false;

            if (newIceCream == null || newIceCream.Price <= 0 || string.IsNullOrWhiteSpace(newIceCream.Name) || newIceCream.Id != id)
                return false;

            var iceCream = ItemsList.FirstOrDefault(i => i.Id == id);
            if (iceCream == null)
                return false;

            iceCream.Name = newIceCream.Name;
            iceCream.Price = newIceCream.Price;
            saveToFile();
            return true;
        }

        private bool isValidRequest(int id)
        {
            var item = ItemsList.FirstOrDefault(i => i.Id == id);
            return item == null || activeUserService.UserId == item.AgentId || activeUserService.Type == "Admin";
        }
}
