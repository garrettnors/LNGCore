using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CustomerController(IInvoiceRepository invoiceRepository, IEventRepository eventRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _eventRepository = eventRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
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

            var customers = _customerRepository.GetAllCustomers(searchTerm).ToList();
            pagination.NumberOfPages =
                customers.Count <= take ? 1 : (int)Math.Ceiling(customers.Count / (decimal)take);

            vm.Customers = customers.Skip(skip).Take(take).ToList();
            vm.PaginationParameters = pagination;
            vm.SearchTerm = searchTerm;
            return View(vm);
        }

        [HttpGet]
        public IActionResult EditCustomer(int customerId)
        {
            var customer = _customerRepository.GetCustomer(customerId);

            if (customer.Id == 0)
                customer.Taxable = true;

            var vm = _mapper.Map<EditCustomerViewModel>(customer);
            return PartialView("_EditCustomer", vm);
        }

        [HttpPost]
        public IActionResult EditCustomer(EditCustomerViewModel model)
        {
            var customer = _mapper.Map<ICustomer>(model);
            _customerRepository.SaveCustomer(customer);
            return RedirectToAction("Index");
        }
    }
}