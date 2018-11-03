using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.UI.Hubs;
using LNGCore.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LNGCore.UI.Controllers
{
    public class CustomOrderController : Controller
    {
        public IOrnamentOrderRepository ornamentRepo;
        public CustomOrderController(IOrnamentOrderRepository ornamentOrderRepository)
        {
            ornamentRepo = ornamentOrderRepository;
        }

        public IActionResult Index()
        {
            var vm = new CustomOrderViewModel();

            if (HttpContext.Session.GetString("Ornaments") != null)
                vm = JsonConvert.DeserializeObject<CustomOrderViewModel>(HttpContext.Session.GetString("Ornaments"));
            else
            {
                HttpContext.Session.SetString("Ornaments", JsonConvert.SerializeObject(vm));
            }

            return View(vm);
        }

        public async Task<IActionResult> SaveOrder()
        {
            var ornaments = JsonConvert.DeserializeObject<CustomOrderViewModel>(HttpContext.Session.GetString("Ornaments"));
            ornamentRepo.SaveOrnamentOrder(ornaments.ExistingOrders.Cast<IOrnamentOrders>().ToList());

            HttpContext.Session.Remove("Ornaments");
            TempData["SuccessBannerMessage"] = "We've received your order! You'll get a confirmation email soon.";
            await UpdateOrnamentCounts();
            return RedirectToAction("Index");
        }

        public async Task UpdateOrnamentCounts()
        {
            var hub = new OrnamentCountHub();
            var random = new Random();
            //await hub.SendOrnamentCount("1", random.Next(200));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrnament(CustomOrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var ornamentSession = JsonConvert.DeserializeObject<CustomOrderViewModel>(HttpContext.Session.GetString("Ornaments"));

                order.NewOrder.Id = ornamentSession.ExistingOrders.Count + 1;
                ornamentSession.ExistingOrders.Add(order.NewOrder);
                order.ExistingOrders = ornamentSession.ExistingOrders;

                HttpContext.Session.SetString("Ornaments", JsonConvert.SerializeObject(order));
            }

            await UpdateOrnamentCounts();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveOrnament(int id)
        {
            var ornaments = HttpContext.Session.GetString("Ornaments");

            var deserializedOrnaments = ornaments == null
                ? new CustomOrderViewModel()
                : JsonConvert.DeserializeObject<CustomOrderViewModel>(ornaments);

            var ornament = deserializedOrnaments.ExistingOrders.FirstOrDefault(f => f.Id == id);

            if (ornament == null)
                return RedirectToAction("Index");

            deserializedOrnaments.ExistingOrders.Remove(ornament);
            HttpContext.Session.SetString("Ornaments", JsonConvert.SerializeObject(deserializedOrnaments));

            await UpdateOrnamentCounts();
            return RedirectToAction("Index");
        }
    }
}