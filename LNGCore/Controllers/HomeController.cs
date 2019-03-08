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
        private readonly ILogRepository _logRepository;
        private readonly IConfiguration _config;
        public HomeController(IBillSheetRepository billSheetRepositoryParam, ICustomerRepository customerRepositoryParam, ILogRepository logRepositoryParam, IConfiguration configParam)
        {
            _billSheetRepository = billSheetRepositoryParam;
            _customerRepository = customerRepositoryParam;
            _logRepository = logRepositoryParam;
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
            var etsyKey = _config.GetSection("SiteConfiguration")["EtsyAPIKey"];

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
            var vm = new SendContactMessageViewModel
            {
                GooglePublicKey = _config.GetSection("SiteConfiguration")["RecaptchaSitePublic"]
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SendContactMessage(SendContactMessageViewModel model)
        {
            var errorMsg = string.Empty;

            if (ModelState.IsValid)
            {
                var verify = new RecaptchaVerify(_config, _logRepository);
                var googleResponse = await verify.GetRecaptchaScore(model.GoogleClientToken);
                if (googleResponse.success)
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
                        RecipientEmail = _config.GetSection("SiteConfiguration")["SiteEmail"],
                        RecipientDisplayName = _config.GetSection("SiteConfiguration")["SiteName"]
                    };

                    var error = email.SendEmail();
                    if (!string.IsNullOrWhiteSpace(error))
                    {

                        errorMsg = $"Email not sent - {error}";
                    }
                }
                else
                {
                    errorMsg = $"RECAPTCHA failed - score {googleResponse.score}";
                }
            }
            else
            {
                errorMsg = $"Model State Invalid: {string.Join('|', ModelState.Values.Select(s => s.Errors))}";
            }

            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                var log = _logRepository.GetLog(0);
                log.Date = DateTime.Now;
                log.LogType = "Error";
                log.Summary = errorMsg;             
                _logRepository.SaveLog(log);
                TempData["ErrorBannerMessage"] = "Your correspondence didn't reach us, please email us directly at Info@LNGLaserworks.com for immediate assistance.";
            }

            else
                TempData["SuccessBannerMessage"] = "We have received your message and will get back with you as soon as we can!";


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
