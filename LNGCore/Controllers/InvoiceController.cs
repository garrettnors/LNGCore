namespace LNGCore.UI.Controllers
{
    using AutoMapper;
    using LNGCore.Domain.Abstract.Class;
    using LNGCore.Domain.Abstract.Repository;
    using LNGCore.UI.Enums;
    using LNGCore.UI.Models.Admin;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;

        private readonly IEventRepository _eventRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IMapper _mapper;

        public InvoiceController(IInvoiceRepository invoiceRepository, IEventRepository eventRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _eventRepository = eventRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public IActionResult Index(InvoiceTypeEnum type = InvoiceTypeEnum.Invoice, int page = 1, int take = 20,
            string searchTerm = "")
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["controller"];


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
                CurrentPage = skip / take + 1
            };

            List<IInvoice> items;
            string viewTitle;

            switch (type)
            {
                case InvoiceTypeEnum.Invoice:
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
            var invoiceItem = _mapper.Map<InvoiceItem>(invoice);

            var vm = new EditInvoiceViewModel
            {
                Invoice = invoiceItem,
                Customers = _customerRepository.GetAllCustomers().ToList(),
                Employees = _employeeRepository.GetEmployees().ToList()
            };

            if (invoice.Voided)
                vm.InvoiceType = InvoiceTypeEnum.Voided;
            else if (invoice.IsQuote)
                vm.InvoiceType = InvoiceTypeEnum.Quote;
            else if (invoice.IsDonated == true)
                vm.InvoiceType = InvoiceTypeEnum.Donated;
            else if (invoice.IsPaid == true)
                vm.InvoiceType = InvoiceTypeEnum.Paid;
            else
                vm.InvoiceType = InvoiceTypeEnum.Invoice;

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditInvoice(EditInvoiceViewModel model)
        {
            model.Invoice.Voided = false;
            model.Invoice.IsPaid = false;
            model.Invoice.IsDonated = false;
            model.Invoice.IsQuote = false;
            model.Invoice.CompletedBy = _employeeRepository.GetEmployee(model.Invoice.EmployeeId).EmpName;

            switch (model.InvoiceType)
            {
                case InvoiceTypeEnum.Invoice:
                    break;
                case InvoiceTypeEnum.Paid:
                    model.Invoice.IsPaid = true;
                    model.Invoice.PaidDate = DateTime.Now;
                    break;
                case InvoiceTypeEnum.Donated:
                    model.Invoice.IsDonated = true;
                    break;
                case InvoiceTypeEnum.Voided:
                    model.Invoice.Voided = true;
                    break;
                case InvoiceTypeEnum.Quote:
                    model.Invoice.IsQuote = true;
                    break;
                default:
                    break;
            }

            _invoiceRepository.SaveAttachmentsToInvoice(9999, model.UploadedFiles, false);
            _invoiceRepository.SaveAttachmentsToInvoice(9999, model.UploadedProofs, true);

            var saveInvoice = _mapper.Map<IInvoice>(model.Invoice);
            //var invoiceId = _invoiceRepository.SaveInvoice(saveInvoice);

            var saveLines = _mapper.Map<List<ILineItem>>(model.LineItems.Where(w => w.Quantity > 0));

            //_invoiceRepository.SaveLineItems(saveLines, invoiceId);

            return RedirectToAction("Index", new { type = model.InvoiceType });
        }

        public IActionResult GetInvoiceLines(int invoiceId, int startingIndex)
        {
            const int linesPerGet = 5;
            var vm = new LineItemViewModel
            {
                LineItems = _invoiceRepository.GetLineItems(invoiceId, startingIndex, linesPerGet).ToList(),
                LineIndex = startingIndex,
                InvoiceId = invoiceId,
                ItemTypes = _invoiceRepository.GetItemTypes().ToList()
            };

            return PartialView("_InvoiceLineItem", vm);
        }

        public PartialViewResult GetLineItemSuggestions(LineItemSuggestionViewModel model)
        {
            if (model.CustomerId > 0)
                model.CustomerLineItems = _invoiceRepository.GetLineItems(model.SearchTerm, model.CustomerId).ToList();

            model.OverallLineItems = _invoiceRepository.GetLineItems(model.SearchTerm, model.CustomerId).ToList();

            return PartialView("_LineItemSuggestions", model);
        }

        public IActionResult MarkInvoicePaid(int invoiceId = 0)
        {
            var success = _invoiceRepository.MarkInvoicePaid(invoiceId);
            return StatusCode((int)(success ? HttpStatusCode.OK : HttpStatusCode.Conflict));
        }

        public IActionResult ViewInvoice(int invoiceId)
        {
            var invoice = _invoiceRepository.GetInvoice(invoiceId);           
            return View(invoice);
        }

        public IActionResult GetInvoicePdf(int invoiceId)
        {
            return null;
        }
    }
}
