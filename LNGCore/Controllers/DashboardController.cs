using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using LNGCore.Services.Abstract.Class;
using LNGCore.Services.Abstract.Repository;
using LNGCore.UI.Enums;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public DashboardController(IInvoiceRepository invoiceRepository, IEventRepository eventRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _eventRepository = eventRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["controller"];

            var vm = new DashboardViewModel
            {
                Events = _eventRepository.GetUpcomingEvents().ToList(),
                YtdSales = _invoiceRepository.GetYearToDateSales().Sum(s => s.InvoiceTotal) ?? 0,
                OpenInvoiceAmount = _invoiceRepository.GetOpenInvoices().Sum(s => s.InvoiceTotal) ?? 0,
                PastDueAmount = _invoiceRepository.GetPastDueInvoices().Sum(s => s.InvoiceTotal) ?? 0
            };
            return View(vm);
        }

    }
}