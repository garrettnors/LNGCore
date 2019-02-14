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
        public AdminController(IInvoiceRepository invoiceRepository, IEventRepository eventRepository)
        {
            _invoiceRepository = invoiceRepository;
            _eventRepository = eventRepository;
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

        public IActionResult Invoices(InvoiceTypeEnum type = InvoiceTypeEnum.Open, string searchTerm = "")
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["action"];

            var vm = new InvoiceViewModel
            {
                InvoiceType = type,
                SearchTerm = searchTerm
            };
            return View(vm);
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

        public PartialViewResult GetInvoicePartial(InvoiceTypeEnum invoiceType = InvoiceTypeEnum.Open, int take = 20, int page = 1, string searchTerm = "")
        {
            var skip = take * (page - 1);
            var vm = new InvoiceViewModel
            {
                InvoiceType = invoiceType,
                SearchTerm = searchTerm
            };

            var pagination = new PaginationViewModel
            {
                InvoiceType = invoiceType,
                Take = take,
                CurrentPage = skip == 0 ? 1 : skip / take + 1
            };

            List<IInvoice> items;
            string viewName;

            switch (invoiceType)
            {

                case InvoiceTypeEnum.Open:
                    items = _invoiceRepository.GetOpenInvoices(searchTerm).ToList();
                    viewName = "_OpenInvoices";
                    break;

                case InvoiceTypeEnum.Paid:
                    items = _invoiceRepository.GetPaidInvoices(searchTerm).ToList();
                    viewName = "_PaidInvoices";
                    break;

                case InvoiceTypeEnum.Donated:
                    items = _invoiceRepository.GetDonatedItems(searchTerm).ToList();
                    viewName = "_DonatedItems";
                    break;

                case InvoiceTypeEnum.Voided:
                    items = _invoiceRepository.GetVoidedItems(searchTerm).ToList();
                    viewName = "_VoidedItems";
                    break;

                case InvoiceTypeEnum.Quote:
                    items = _invoiceRepository.GetOpenQuotes(searchTerm).ToList();
                    viewName = "_OpenQuotes";
                    break;

                default:
                    throw new NotImplementedException();
            }

            pagination.NumberOfPages = items.Count <= take ? 1 : (int)Math.Ceiling(items.Count / (decimal)take);
            vm.Invoices = items.Skip(skip).Take(take).ToList();
            vm.PaginationParameters = pagination;
            return PartialView(viewName, vm);
        }
    }
}