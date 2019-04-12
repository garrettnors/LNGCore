using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Enums;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                Events = _eventService.GetUpcomingEvents().ToList(),
                YtdSales = _invoiceService.GetYearToDateSales().Sum(s => s.InvoiceTotal) ?? 0,
                OpenInvoiceAmount = _invoiceService.GetOpenInvoices().Sum(s => s.InvoiceTotal) ?? 0,
                PastDueAmount = _invoiceService.GetPastDueInvoices().Sum(s => s.InvoiceTotal) ?? 0
            };
            return View(vm);
        }

    }
}