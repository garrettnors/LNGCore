using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LNGCore.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using LNGCore.Infrastructure;
using LNGCore.Domain.Concrete;
using LNGCore.Domain.Abstract.Repository;
using Newtonsoft.Json;
using LNGCore.UI.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace LNGCore.Controllers
{

    public class HomeController : Controller
    {
        private readonly IBillSheetRepository billSheetRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IConfiguration config;
        public HomeController(IBillSheetRepository billSheetRepositoryParam, ICustomerRepository customerRepositoryParam, IConfiguration configParam)
        {
            billSheetRepository = billSheetRepositoryParam;
            customerRepository = customerRepositoryParam;
            config = configParam;
        }
        public async Task<IActionResult> Index()
        {
            var vm = new IndexViewModel
            {
                EtsyListing = await GetEtsyListings(),
                Customers = customerRepository.GetAllCustomers()
            };
            return View(vm);
        }

        public string GetBills()
        {
            return JsonConvert.SerializeObject(billSheetRepository.GetAllBills());
        }
        public string GetCustomers()
        {
            return string.Empty;// JsonConvert.SerializeObject(customerRepository.GetAllCustomers());
        }
        public string GetCustomerFromInvoice()
        {
            return JsonConvert.SerializeObject(customerRepository.GetCustomer(2));
        }
        public async Task<EtsyListings> GetEtsyListings()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var etsyKey = config["SiteConfiguration:EtsyAPIKey"];
            var response = await client.GetAsync($"https://openapi.etsy.com/v2/shops/hoppergator/listings/active?api_key={etsyKey}&includes=Images");
            var model = response.Content.ReadAsAsync<EtsyListings>();
            return await model;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult SendContactMessage(SendContactMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorBannerMessage"] = "Please email us directly at Info@LNGLaserworks.com for immediate assistance.";
            }
            else
            {
                TempData["SuccessBannerMessage"] = "We have received your message and will get back with you as soon as we can!";
            }

            return RedirectToAction("Contact");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
