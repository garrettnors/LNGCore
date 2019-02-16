using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.UI.Enums;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public AdminController(IInvoiceRepository invoiceRepository, IEventRepository eventRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
        {
            _invoiceRepository = invoiceRepository;
            _eventRepository = eventRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["action"];

            var vm = new DashboardViewModel
            {
                Events = _eventRepository.GetUpcomingEvents().ToList(),
                YtdSales = _invoiceRepository.GetYearToDateSales().Sum(s => s.InvoiceTotal) ?? 0,
                OpenInvoiceAmount = _invoiceRepository.GetOpenInvoices().Sum(s => s.InvoiceTotal) ?? 0,
                PastDueAmount = _invoiceRepository.GetPastDueInvoices().Sum(s => s.InvoiceTotal) ?? 0
            };
            return View(vm);
        }

        public IActionResult Invoices(InvoiceTypeEnum type = InvoiceTypeEnum.Open, int page = 1, int take = 20, string searchTerm = "")
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["action"];


            var skip = take * (page - 1);
            var vm = new InvoiceViewModel
            {
                InvoiceType = type,
                SearchTerm = searchTerm
            };

            var pagination = new PaginationViewModel
            {
                InvoiceType = type,
                Take = take,
                CurrentPage = skip == 0 ? 1 : skip / take + 1
            };

            List<IInvoice> items;
            string viewTitle;

            switch (type)
            {

                case InvoiceTypeEnum.Open:
                    items = _invoiceRepository.GetOpenInvoices(searchTerm).ToList();
                    viewTitle = "Open Invoices";
                    break;

                case InvoiceTypeEnum.Paid:
                    items = _invoiceRepository.GetPaidInvoices(searchTerm).ToList();
                    viewTitle = "Paid Invoices";
                    break;

                case InvoiceTypeEnum.Donated:
                    items = _invoiceRepository.GetDonatedItems(searchTerm).ToList();
                    viewTitle = "Donated Items";
                    break;

                case InvoiceTypeEnum.Voided:
                    items = _invoiceRepository.GetVoidedItems(searchTerm).ToList();
                    viewTitle = "Voided Items";
                    break;

                case InvoiceTypeEnum.Quote:
                    items = _invoiceRepository.GetOpenQuotes(searchTerm).ToList();
                    viewTitle = "Open Quotes";
                    break;

                default:
                    throw new NotImplementedException();
            }

            pagination.NumberOfPages = items.Count <= take ? 1 : (int)Math.Ceiling(items.Count / (decimal)take);
            vm.Invoices = items.Skip(skip).Take(take).ToList();
            vm.ViewTitle = viewTitle;
            vm.PaginationParameters = pagination;

            return View(vm);
        }

        public IActionResult EditInvoice(int invoiceId = 0)

        {
            var invoice = _invoiceRepository.GetInvoice(invoiceId);

            var invoiceItem = new InvoiceItem
            {
                Id = invoice.Id,
                CompletedBy = invoice.CompletedBy,
                Customer = invoice.Customer,
                CustomerId = invoice.CustomerId,
                OrderDate = invoice.OrderDate,
                LineItem = invoice.LineItem,
                IsDonated = invoice.IsDonated,
                IsPaid = invoice.IsPaid,
                EmployeeId = invoice.EmployeeId,
                Deadline = invoice.Deadline,
                IsQuote = invoice.IsQuote,
                PaidDate = invoice.PaidDate,
                Voided = invoice.Voided,
                Pofield = invoice.Pofield,
                ShipCost = invoice.ShipCost,
                Notes = invoice.Notes,
                InvoiceProofUrl = invoice.InvoiceProofUrl,
                ShippingMethod = invoice.ShippingMethod,
                TaxPercent = invoice.TaxPercent
            };

            var vm = new EditInvoiceViewModel
            {
                Invoice = invoiceItem,
                Customers = _customerRepository.GetAllCustomers().ToList(),
                Employees = _employeeRepository.GetEmployees().ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditInvoice(EditInvoiceViewModel model)
        {
            var invoice = _invoiceRepository.GetInvoice(model.Invoice.Id);
            invoice.CustomerId = model.Invoice.CustomerId;
            invoice.OrderDate = model.Invoice.OrderDate;
            invoice.CompletedBy = model.Invoice.CompletedBy;
            invoice.Deadline = model.Invoice.Deadline;
            invoice.EmployeeId = model.Invoice.EmployeeId;
            invoice.InvoiceProofUrl = model.Invoice.InvoiceProofUrl;
            invoice.IsDonated = model.Invoice.IsDonated;
            invoice.IsPaid = model.Invoice.IsPaid;
            invoice.PaidDate = model.Invoice.PaidDate;
            invoice.IsQuote = model.Invoice.IsQuote;
            invoice.Notes = model.Invoice.Notes;
            invoice.Pofield = model.Invoice.Pofield;
            invoice.Voided = model.Invoice.Voided;
            invoice.ShippingMethod = model.Invoice.ShippingMethod;
            invoice.ShipCost = model.Invoice.ShipCost;
            var invoiceId = _invoiceRepository.SaveInvoice(invoice);

            var saveLines = new List<ILineItem>();
            var lineItems = model.LineItems.Where(w => w.Quantity > 0);



            _invoiceRepository.SaveLineItems(saveLines, invoiceId);

            return RedirectToAction("Index");
        }

        public IActionResult GetInvoiceLines(int invoiceId, int startingIndex)
        {
            var linesPerGet = 5;
            var vm = new LineItemViewModel
            {
                LineItems = _invoiceRepository.GetLineItems(invoiceId, startingIndex, 5).ToList(),
                LineIndex = startingIndex,
                InvoiceId = invoiceId,
                ItemTypes = _invoiceRepository.GetItemTypes().ToList()
            };

            return PartialView("_InvoiceLineItem", vm);
        }

        public IActionResult Customers()
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["action"];
            return View();
        }

        public IActionResult MarkInvoicePaid(int invoiceId = 0)
        {
            var success = _invoiceRepository.MarkInvoicePaid(invoiceId);
            return StatusCode((int)(success ? HttpStatusCode.OK : HttpStatusCode.Conflict));
        }

        //public PartialViewResult GetInvoicePartial(InvoiceTypeEnum invoiceType = InvoiceTypeEnum.Open, int take = 20, int page = 1, string searchTerm = "")
        //{

        //    return PartialView("_InvoiceItems", vm);
        //}


    }
}