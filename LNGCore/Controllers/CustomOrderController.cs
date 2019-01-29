using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Logical;
using LNGCore.UI.Hubs;
using LNGCore.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LNGCore.UI.Controllers
{
    public class CustomOrderController : Controller
    {
        private readonly IOrnamentOrderRepository _ornamentRepo;
        private readonly ILogRepository _logRepository;
        private readonly IConfiguration _config;
        public CustomOrderController(IOrnamentOrderRepository ornamentOrderRepository, ILogRepository logRepository, IConfiguration config)
        {
            _ornamentRepo = ornamentOrderRepository;
            _logRepository = logRepository;
            _config = config;
        }

        public IActionResult Index()
        {
            //var vm = new CustomOrderViewModel();

            //if (HttpContext.Session.GetString("Ornaments") != null)
            //    vm = JsonConvert.DeserializeObject<CustomOrderViewModel>(HttpContext.Session.GetString("Ornaments"));
            //else
            //{
            //    HttpContext.Session.SetString("Ornaments", JsonConvert.SerializeObject(vm));
            //}

            //vm.GooglePublicKey = _config.GetSection("SiteConfiguration")["RecaptchaSitePublic"];
            //return View(vm);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SaveOrder(string googleToken)
        {
            var verify = new RecaptchaVerify(_config, _logRepository);
            var googleResponse = await verify.GetRecaptchaScore(googleToken);
            if (googleResponse.success)
            {
                var ornaments = JsonConvert.DeserializeObject<CustomOrderViewModel>(HttpContext.Session.GetString("Ornaments"));
                var ornList = ornaments.ExistingOrders.Cast<IOrnamentOrders>().ToList();
                _ornamentRepo.SaveOrnamentOrder(ornList);
                SendOrderNotification(ornList);
                HttpContext.Session.Remove("Ornaments");
                TempData["SuccessBannerMessage"] = "We've received your order! You'll get a confirmation email soon.";
                await UpdateOrnamentCounts();
            }
            else
            {
                TempData["ErrorBannerMessage"] = "Something went wrong! Please check over your order and try submitting again.";
            }
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

        private void SendOrderNotification(List<IOrnamentOrders> orderList)
        {
            var mail = new Email(_config)
            {
                MailSubject = "New Ornament Order",
                Message = "The following was ordered:<br>"
            };
            var thStyle = "style=\"text-align:left;\"";
            mail.Message += "<table style=\"border:1px solid #333; min-width: 800px;word-wrap: break-word;\">";
            mail.Message += "<thead>";
            mail.Message += "<tr>";
            mail.Message += $"<th {thStyle}>Amount</th>";
            mail.Message += $"<th {thStyle}>Style</th>";
            mail.Message += $"<th {thStyle}>Design</th>";
            mail.Message += $"<th {thStyle}>Instructions</th>";
            mail.Message += $"<th {thStyle}>User Name</th>";
            mail.Message += $"<th {thStyle}>User Email</th>";
            mail.Message += "</tr>";
            mail.Message += "</thead>";
            mail.Message += "<tbody>";

            foreach (var order in orderList)
            {
                mail.Message += "<tr>";
                mail.Message += $"<td>{order.Amount}</td>";
                mail.Message += $"<td>{order.OrnamentStyle}</td>";
                mail.Message += $"<td>{order.OrnamentDesign}</td>";
                mail.Message += $"<td>{order.SpecialInstructions}</td>";
                mail.Message += $"<td>{order.UserName}</td>";
                mail.Message += $"<td>{order.UserEmail}</td>";
                mail.Message += "</tr>";
            }

            mail.Message += "</tbody>";

            mail.RecipientEmail = _config.GetSection("SiteConfiguration")["SiteEmail"];
            mail.SenderDisplayName = _config.GetSection("SiteConfiguration")["SiteName"];
            mail.SenderEmail = _config.GetSection("SiteConfiguration")["SiteEmail"];
            mail.SendEmail();
        }
    }
}