namespace LNGCore.UI.Controllers
{
    using AutoMapper;
    using LNGCore.Services.Abstract.Class;
    using LNGCore.Services.Abstract.Repository;
    using LNGCore.UI.Enums;
    using LNGCore.UI.Models.Admin;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Rotativa.AspNetCore;
    using System.Net.Mail;
    using System.IO;

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

            if (invoice == null)
                return RedirectToAction("Index");

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
            if (ModelState.IsValid)
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


                var saveInvoice = _mapper.Map<IInvoice>(model.Invoice);
                var invoiceId = _invoiceRepository.SaveInvoice(saveInvoice);
                var saveLines = _mapper.Map<List<ILineItem>>(model.LineItems.Where(w => w.Quantity > 0));

                _invoiceRepository.SaveLineItems(saveLines, invoiceId);
                //_invoiceRepository.SaveAttachmentsToInvoice(invoiceId, model.UploadedFiles, false);
                _invoiceRepository.SaveAttachmentsToInvoice(invoiceId, model.UploadedProofs, true);
            }
            TempData["ErrorBannerMessage"] = "The invoice could not be saved. Please double-check all data and try again.";
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
                model.CustomerLineItems = _invoiceRepository.GetLineItems(model.ItemId, model.CustomerId, true).ToList();

            model.OverallLineItems = _invoiceRepository.GetLineItems(model.ItemId, model.CustomerId, false).ToList();

            return PartialView("_LineItemSuggestions", model);
        }

        public IActionResult MarkInvoicePaid(int invoiceId = 0)
        {
            var success = _invoiceRepository.MarkInvoicePaid(invoiceId);
            return StatusCode((int)(success ? HttpStatusCode.OK : HttpStatusCode.Conflict));
        }

        public IActionResult ViewInvoice(int invoiceId)
        {
            var vm = new ViewInvoiceViewModel
            {
                Invoice = _invoiceRepository.GetInvoice(invoiceId),
                InvoiceData = GetInvoicePdf(invoiceId)
            };
            return View(vm);
        }

        public IActionResult SendInvoiceEmail(SendInvoiceViewModel vm)
        {
            TempData["SuccessBannerMessage"] = "An email has been sent to the recipient(s) you selected.";
            return RedirectToAction("ViewInvoice", new { invoiceId = vm.InvoiceId });
        }

        public string GetInvoicePdf(int invoiceId)
        {
            const int itemsPerPageMax = 20;
            const string footer = "--footer-center \"Thank you for choosing LNG Laserworks, we appreciate your business!\" " +
                "--footer-line --footer-font-size \"12\" --footer-font-name \"calibri light\"";

            var invoice = _invoiceRepository.GetInvoice(invoiceId);
           var model = new ViewInvoicePdfViewModel
            {
                DocTitle = invoiceId.ToString(),
                Invoice = invoice,
                RowsPerPage = itemsPerPageMax,
                TotalLineItems = invoice.LineItem.Count()
            };

            var actionPdf = new ViewAsPdf("InvoicePdf", model)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                CustomSwitches = footer
            };
            //return actionPdf.BuildFile(ControllerContext).Result;

            var byteArray = actionPdf.BuildFile(ControllerContext).Result;
            return Convert.ToBase64String(byteArray, 0, byteArray.Length);

            //var actionPDF = new ViewAsPdf("GetInvoicePdf", model) //some route values)
            //{
            //    PageSize = Rotativa.AspNetCore.Options.Size.Letter,
            //    FileName = "TestInvoice.pdf"
            //    //FileName = "TestView.pdf",
            //    //PageSize = "",
            //    //PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    // PageMargins = { Left = 1, Right = 1 }
            //};
            //System.Threading.Tasks.Task<byte[]> applicationPDFData = actionPDF.BuildFile(ControllerContext);

            //var message = new MailMessage(new MailAddress("info@lnglaserworks.com", "LNG Laserworks"), new MailAddress("garrett.nors@gmail.com"))
            //{
            //    Subject = "Test Invoice Send",
            //    Body = "test email"
            //};
            //message.Attachments.Add(new Attachment(new MemoryStream(applicationPDFData.Result), actionPDF.FileName));
            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    EnableSsl = true,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("mailer@lnglaserworks.com", "pisrmvwifrjgefcp"),
            //    DeliveryMethod = SmtpDeliveryMethod.Network
            //};
            //client.Send(message);

            //return View(model);            
        }
    }
}
