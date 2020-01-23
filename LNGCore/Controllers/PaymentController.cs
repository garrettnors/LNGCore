using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;

namespace LNGCore.UI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IInvoiceService _invoiceService;
        private readonly ILogService _logService;
        private readonly ChargeService _chargeService;
        public PaymentController(IInvoiceService invoiceService, ILogService logService, IConfiguration config)
        {
            _invoiceService = invoiceService;
            _config = config;
            _logService = logService;
            _chargeService = new ChargeService();

            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = _config.GetSection("SiteConfiguration")["StripeSecretKey"];
        }
        public IActionResult Index(string id = null)
        {
            var invoice = _invoiceService.GetByIdentifierGuid(id);

            if (invoice == null || invoice.Id == 0)
                return RedirectToAction("NoInvoice");

            var vm = new PaymentViewModel
            {
                Invoice = invoice,
                StripePubKey = _config.GetSection("SiteConfiguration")["StripePublishibleKey"]
            };

            return View("Index", vm);
        }

        public IActionResult NoInvoice()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Charge(ChargeModel model)
        {
            var invoice = _invoiceService.GetByIdentifierGuid(model.InvoiceIdentifier);
            if (invoice == null || invoice.Id == 0)
                return RedirectToAction("NoInvoice");

            var token = model.StripeToken;
            
            var subtotal = invoice.LineItem.Sum(s => s.Quantity * s.ItemPrice) ?? 0;
            var tax = invoice.LineItem.Sum(s => s.TaxAmount);

            var total = subtotal + tax + (invoice.ShipCost ?? 0);

            var options = new ChargeCreateOptions
            {
                Amount = (long)(total * 100), //stripe takes LONG, convert decimal
                Currency = "usd",
                Description = $"Invoice #{invoice.Id} for {invoice.Customer.DisplayName}",
                Metadata = new Dictionary<string, string> { { "InvoiceIdentifier", invoice.Identifier } },
                Source = token
            };

            var customerEmail = invoice.Customer.Email ?? invoice.Customer.SecondaryEmail;

            if (!string.IsNullOrWhiteSpace(customerEmail))
                options.ReceiptEmail = customerEmail;
                        
            Charge charge = _chargeService.Create(options);

            if (charge == null)
                return RedirectToAction("NoInvoice");

            var log = _logService.Get(0);
            log.Date = DateTime.Now;
            log.LogType = "StripePaymentInfo";
            log.InvoiceId = invoice.Id;
            log.Summary = JsonConvert.SerializeObject(charge);
            _logService.Add(log);

            if (charge.Paid)
                _invoiceService.SetInvoiceStatus(invoice.Id, Domain.Infrastructure.Enums.InvoiceTypeEnum.Paid, charge.Id);

            return View("ChargeResult", charge);
        }

        public IActionResult ChargeResult(string chargeId)
        {
            try
            {
                Charge charge = _chargeService.Get(chargeId);
                return View(charge);
            }
            catch (Exception e)
            {
                var log = _logService.Get(0);
                log.Date = DateTime.Now;
                log.LogType = "Error Occurred";
                log.Summary = $"Charge ID:{chargeId} - {JsonConvert.SerializeObject(e.ToString())}";
                _logService.Add(log);

                return View(new Charge());
            }
        }
    }
}