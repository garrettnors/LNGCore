using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LNGCore.UI.Controllers
{
    public class CustomOrderController : Controller
    {
        public IActionResult Index()
        {
            var vm = new List<CustomOrderViewModel>();
            if (HttpContext.Session.GetString("Ornaments") != null)
                vm = JsonConvert.DeserializeObject<List<CustomOrderViewModel>>(
                    HttpContext.Session.GetString("Ornaments"));

            //var vm = JsonConvert.DeserializeObject<List<CustomOrderViewModel>>(TempData["Ornaments"].ToString());

            return View(vm);
        }

        public IActionResult AddOrnament(CustomOrderViewModel model)
        {
            var ornaments = HttpContext.Session.GetString("Ornaments");

            var deserializedOrnaments = ornaments == null
                ? new List<CustomOrderViewModel>()
                : JsonConvert.DeserializeObject<List<CustomOrderViewModel>>(ornaments);
            model.Id = deserializedOrnaments.Count + 1;
            deserializedOrnaments.Add(model);
            HttpContext.Session.SetString("Ornaments", JsonConvert.SerializeObject(deserializedOrnaments));

            return RedirectToAction("Index");
        }

        public IActionResult RemoveOrnament(int id)
        {
            var ornaments = HttpContext.Session.GetString("Ornaments");

            var deserializedOrnaments = ornaments == null
                ? new List<CustomOrderViewModel>()
                : JsonConvert.DeserializeObject<List<CustomOrderViewModel>>(ornaments);

            var ornament = deserializedOrnaments.FirstOrDefault(f => f.Id == id);

            if (ornament == null)
                return RedirectToAction("Index");

            deserializedOrnaments.Remove(ornament);
            HttpContext.Session.SetString("Ornaments", JsonConvert.SerializeObject(deserializedOrnaments));

            return RedirectToAction("Index");
        }
    }
}