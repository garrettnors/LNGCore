using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomerController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IEventService _eventService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;

        public CustomerController(IInvoiceService invoiceService, IEventService eventService,
            ICustomerService customerService, IEmployeeService employeeService)
        {
            _invoiceService = invoiceService;
            _eventService = eventService;
            _customerService = customerService;
            _employeeService = employeeService;
        }
        public IActionResult Index(int page = 1, int take = 15, string searchTerm = "")
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["controller"];

            var vm = new CustomerViewModel();

            var skip = take * (page - 1);
            var pagination = new PaginationViewModel
            {
                Take = take,
                CurrentPage = skip / take + 1
            };

            var customers = _customerService.GetAllCustomers(searchTerm).ToList();
            pagination.NumberOfPages =
                customers.Count <= take ? 1 : (int)Math.Ceiling(customers.Count / (decimal)take);

            vm.Customers = customers.Skip(skip).Take(take).ToList();
            vm.PaginationParameters = pagination;
            vm.SearchTerm = searchTerm;

            return View(vm);
        }

        [HttpGet]
        public IActionResult EditCustomer(int customerId, bool fromAjax = false)
        {
            var customer = _customerService.Get(customerId);

            if (customer.Id == 0)
                customer.Taxable = true;

            var vm = new EditCustomerViewModel();
            vm.fromAjax = fromAjax;
            vm.Customer = customer;
            return PartialView("_EditCustomer", vm);
        }

        [HttpPost]
        public IActionResult EditCustomer(EditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorBannerMessage"] = "Customer was not saved. Check to make sure your info was not too long (typically 50 characters or less).";
            }

            if (model.Customer.Id == 0)
            {
                model.Customer.Id = _customerService.Add(model.Customer);
                TempData["SuccessBannerMessage"] = "Customer successfully added.";
            }
            else
            {
                _customerService.Edit(model.Customer);
                TempData["SuccessBannerMessage"] = "Customer successfully updated.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public Tuple<int, string> EditCustomerAjax(EditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return null;

            if (model.Customer.Id == 0)
                model.Customer.Id = _customerService.Add(model.Customer);
            else
                _customerService.Edit(model.Customer);


            return Tuple.Create(model.Customer.Id, model.Customer.DisplayName);
        }
    }
}