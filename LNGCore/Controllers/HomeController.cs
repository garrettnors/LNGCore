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
using System.Text;
using LNGCore.Domain.Logical;
using Microsoft.AspNetCore.Http;

namespace LNGCore.Controllers
{

    public class HomeController : Controller
    {
        private readonly IBillSheetRepository _billSheetRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _config;
        public HomeController(IBillSheetRepository billSheetRepositoryParam, ICustomerRepository customerRepositoryParam, IConfiguration configParam)
        {
            _billSheetRepository = billSheetRepositoryParam;
            _customerRepository = customerRepositoryParam;
            _config = configParam;
        }
        public async Task<IActionResult> Index()
        {
            var vm = new IndexViewModel
            {
                EtsyListing = await GetEtsyListings(),
                Customers = _customerRepository.GetAllCustomers()
            };
            return View(vm);
        }

        public async Task<IActionResult> Search(string searchTerm = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return RedirectToAction("Index");

            var vm = new SearchViewModel
            {
                SearchTerm = searchTerm,
                EtsyListing = await GetEtsyListings(searchTerm)
            };
            return View(vm);
        }

        public string GetBills()
        {
            return JsonConvert.SerializeObject(_billSheetRepository.GetAllBills());
        }
        public string GetCustomers()
        {
            return string.Empty;// JsonConvert.SerializeObject(customerRepository.GetAllCustomers());
        }
        public string GetCustomerFromInvoice()
        {
            return JsonConvert.SerializeObject(_customerRepository.GetCustomer(2));
        }
        public async Task<EtsyListings> GetEtsyListings(string searchTerm = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var etsyKey = _config["SiteConfiguration:EtsyAPIKey"];

            var searchTerms = "";
            if (!string.IsNullOrWhiteSpace(searchTerm))
                searchTerms = $"&keywords={searchTerm}";

            var response = await client.GetAsync($"https://openapi.etsy.com/v2/shops/hoppergator/listings/active?api_key={etsyKey}{searchTerms}&includes=Images");
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
                var msg = new StringBuilder();
                msg.AppendFormat("The following mail is being sent on behalf of '{0}'. ", model.EmailAddress);
                msg.AppendLine();
                msg.Append(model.EmailMessage);


                var email = new Email(_config)
                {
                    MailSubject = $"Contact Us Form Submission from {model.EmailAddress}",
                    Message = msg.ToString(),
                    SenderEmail = model.EmailAddress,
                    SenderDisplayName = model.EmailAddress,
                    RecipientEmail = _config["SiteConfiguration:SiteEmail"],
                    RecipientDisplayName = _config["SiteConfiguration:SiteName"]
                };

                //email.SendEmail();
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
