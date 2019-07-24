using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LNGCore.Domain.Infrastructure.Enums;

namespace LNGCore.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IEventService _eventService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;

        public DashboardController(IInvoiceService invoiceService, IEventService eventService,
            ICustomerService customerService, IEmployeeService employeeService)
        {
            _invoiceService = invoiceService;
            _eventService = eventService;
            _customerService = customerService;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["controller"];

            var vm = new DashboardViewModel
            {
                UpcomingEvents = _eventService.GetUpcomingEvents().ToList(),
                YtdSales = _invoiceService.GetYearToDateSales().Sum(s => s.InvoiceTotal) ?? 0,
                OpenInvoiceAmount = _invoiceService.GetInvoices(InvoiceTypeEnum.Open).Sum(s => s.InvoiceTotal) ?? 0,
                PastDueAmount = _invoiceService.GetInvoices(InvoiceTypeEnum.PastDue).Sum(s => s.InvoiceTotal) ?? 0
            };
            return View(vm);
        }

        public IActionResult AllEvents()
        {
            var vm = _eventService.GetAllEvents().ToList();
            return View(vm);
        }

        [HttpGet]
        public PartialViewResult EditUpcomingEvent(int eventId = 0)
        {
            var eventItem = _eventService.Get(eventId);

            if (eventItem.EventDate == null || eventItem.EventDate == DateTime.MinValue)
                eventItem.EventDate = DateTime.Now;

            return PartialView("_EditUpcomingEvent", eventItem);
        }

        [HttpPost]
        public IActionResult EditUpcomingEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    model.EmployeeId = 1; //replace with logged in user
                    _eventService.Add(model);
                }
                else
                {
                    _eventService.Edit(model);
                }
            }

            if (Request.Headers.Keys.Contains("Referer"))
                return Redirect(Request.Headers["Referer"].ToString());

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult DeleteEvent(int eventId)
        {
            _eventService.Delete(eventId);

            if (Request.Headers.Keys.Contains("Referer"))
                return Redirect(Request.Headers["Referer"].ToString());

            return RedirectToAction("Index", "Dashboard");
        }
    }
}