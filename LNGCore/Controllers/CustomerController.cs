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
        public IActionResult EditCustomer(int customerId, bool showSuccessMessage = false)
        {
            var customer = _customerService.GetCustomer(customerId);

            if (customer.Id == 0)
                customer.Taxable = true;

            var vm = new EditCustomerViewModel();
            vm.Customer = customer;
            vm.ShowSuccessMessage = showSuccessMessage;
            return PartialView("_EditCustomer", vm);
        }

        [HttpPost]
        public Tuple<int, string> EditCustomer(EditCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ShowSuccessMessage ?? false)
                    TempData["SuccessBannerMessage"] = "Customer successfully saved.";

                
                return Tuple.Create(_customerService.SaveCustomer(model.Customer), model.Customer.DisplayName);
            }
            TempData["ErrorBannerMessage"] = "Customer was not saved. Check to make sure your info was not too long (typically 50 characters or less).";
            return null;

        }
    }
}