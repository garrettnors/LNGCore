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
using LNGCore.Domain.Services.Interfaces;
using LNGCore.Domain.Database;
using Microsoft.AspNetCore.Authorization;
using static LNGCore.Domain.Infrastructure.Enums;
using LNGCore.Services.Logical;
using Microsoft.Extensions.Configuration;
using System.Text;
using Rotativa.AspNetCore.Options;

namespace LNGCore.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IEventService _eventService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;
        private readonly ILogService _logService;
        private readonly IConfiguration _config;

        public InvoiceController(IInvoiceService invoiceService, IEventService eventService, ILogService logService,
            ICustomerService customerService, IEmployeeService employeeService, IConfiguration config)
        {
            _invoiceService = invoiceService;
            _eventService = eventService;
            _customerService = customerService;
            _employeeService = employeeService;
            _logService = logService;
            _config = config;
        }

        public IActionResult Index(InvoiceTypeEnum type = InvoiceTypeEnum.Open, int page = 1, int take = 20, string searchTerm = "")
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

            var items = _invoiceService.GetInvoices(type, searchTerm).ToList();
            string viewTitle;

            switch (type)
            {
                case InvoiceTypeEnum.Open:
                    viewTitle = "Open Invoices";
                    break;

                case InvoiceTypeEnum.Paid:
                    viewTitle = "Paid Invoices";
                    break;

                case InvoiceTypeEnum.Donated:
                    viewTitle = "Donated Items";
                    break;

                case InvoiceTypeEnum.Voided:
                    viewTitle = "Voided Items";
                    break;

                case InvoiceTypeEnum.Quote:
                    viewTitle = "Open Quotes";
                    break;
                case InvoiceTypeEnum.PastDue:
                    viewTitle = "Past Due Invoices";
                    break;
                case InvoiceTypeEnum.All:
                    viewTitle = "All Invoices";
                    break;
                default:
                    throw new NotImplementedException();
            }

            pagination.NumberOfPages = items.Count <= take ? 1 : (int)Math.Ceiling(items.Count / (decimal)take);
            vm.Invoices = items.Skip(skip).Take(take).ToList();
            vm.ViewTitle = viewTitle;
            vm.PaginationParameters = pagination;
            vm.InvoiceEmailCounts = _invoiceService.GetEmailCountsForInvoices(vm.Invoices.Select(s => s.Id).ToList());
            return View(vm);
        }

        public IActionResult EditInvoice(int invoiceId = 0, InvoiceTypeEnum invoiceType = InvoiceTypeEnum.Open)
        {
            var invoice = _invoiceService.Get(invoiceId);

            if (invoice == null)
                return RedirectToAction("Index");

            var vm = new EditInvoiceViewModel
            {
                Invoice = invoice,
                Customers = _customerService.GetAllCustomers().ToList(),
                Employees = _employeeService.GetEmployees().ToList()
            };

            if (invoiceId == 0)
            {
                vm.InvoiceType = invoiceType;
            }
            else
            {
                if (invoice.Voided)
                    vm.InvoiceType = InvoiceTypeEnum.Voided;
                else if (invoice.IsQuote)
                    vm.InvoiceType = InvoiceTypeEnum.Quote;
                else if (invoice.IsDonated == true)
                    vm.InvoiceType = InvoiceTypeEnum.Donated;
                else if (invoice.IsPaid == true)
                    vm.InvoiceType = InvoiceTypeEnum.Paid;
                else
                    vm.InvoiceType = InvoiceTypeEnum.Open;
            }

            vm.PreviousInvoiceId = _invoiceService.GetPreviousInvoiceId(invoiceId, vm.InvoiceType);
            vm.NextInvoiceId = _invoiceService.GetNextInvoiceId(invoiceId, vm.InvoiceType);

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditInvoice(EditInvoiceViewModel model)
        {
            var originalId = model.Invoice.Id;

            var keyList = ModelState.Keys.Where(w => w.Contains("LineItems"));
            foreach (var key in keyList)
            {
                ModelState.Remove(key);
            }
            if (ModelState.IsValid)
            {
                model.Invoice.Voided = false;
                model.Invoice.IsPaid = false;
                model.Invoice.IsDonated = false;
                model.Invoice.IsQuote = false;
                model.Invoice.CompletedBy = _employeeService.Get(model.Invoice.EmployeeId).EmpName;

                switch (model.InvoiceType)
                {
                    case InvoiceTypeEnum.Open:
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


                if (model.Invoice.Id == 0)
                {
                    model.Invoice.Id = _invoiceService.Add(model.Invoice);
                }
                else
                {
                    _invoiceService.Edit(model.Invoice);
                }

                var saveLines = model.LineItems.Where(w => w.Quantity > 0).ToList();

                _invoiceService.SaveLineItems(saveLines, model.Invoice.Id);
                //_invoiceService.SaveAttachmentsToInvoice(invoiceId, model.UploadedFiles, false);
                _invoiceService.SaveAttachmentsToInvoice(model.Invoice.Id, model.UploadedProofs, true);
                TempData["SuccessBannerMessage"] = "The invoice has been successfully saved!";

            }
            else
            {
                TempData["ErrorBannerMessage"] = "The invoice could not be saved. Please double-check all data and try again.";
            }


            if (originalId == 0)
                return RedirectToAction("Index", new { type = model.InvoiceType });

            return RedirectToAction("EditInvoice", "Invoice", new { invoiceId = originalId });

        }

        public IActionResult GetInvoiceLines(int invoiceId, int startingIndex)
        {
            const int linesPerGet = 5;
            var vm = new LineItemViewModel
            {
                LineItems = _invoiceService.GetLineItems(invoiceId, startingIndex, linesPerGet).ToList(),
                LineIndex = startingIndex,
                InvoiceId = invoiceId,
                ItemTypes = _invoiceService.GetItemTypes().ToList()
            };

            return PartialView("_InvoiceLineItem", vm);
        }

        [HttpPost]
        public HttpStatusCode SetPaidItems(List<int> items, bool isPaid = false)
        {
            if (items.Any())
            {
                _invoiceService.SetParticipantPaidStatus(items, isPaid);
                TempData["SuccessBannerMessage"] = $"Item{(items.Count == 1 ? "" : "s")} #{string.Join(",", items)} paid status set to {isPaid.ToString()}";
            }

            return HttpStatusCode.OK;
        }

        public PartialViewResult GetLineItemSuggestions(LineItemSuggestionViewModel model)
        {
            if (model.CustomerId > 0)
                model.CustomerLineItems = _invoiceService.GetLineItems(model.ItemId, model.CustomerId, true).ToList();

            model.OverallLineItems = _invoiceService.GetLineItems(model.ItemId, model.CustomerId, false).ToList();

            return PartialView("_LineItemSuggestions", model);
        }

        public PartialViewResult GetInvoiceEmailHistory(int invoiceId)
        {
            var history = _logService.GetLogsByInvoiceId(invoiceId).ToList();

            return PartialView("_EmailHistory", history);
        }

        public IActionResult SetInvoiceStatus(int invoiceId = 0, InvoiceTypeEnum status = InvoiceTypeEnum.Open)
        {
            try
            {
                _invoiceService.SetInvoiceStatus(invoiceId, status);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            return StatusCode((int)HttpStatusCode.OK);
        }

        public IActionResult ViewInvoice(int invoiceId)
        {
            var vm = new ViewInvoiceViewModel
            {
                Invoice = _invoiceService.Get(invoiceId)
            };
            return View(vm);
        }

        public IActionResult SendInvoiceEmail(SendInvoiceViewModel vm)
        {
            var errorList = new List<string>();
            var recipients = new List<string>();

            if (vm.SendToPrimary && !string.IsNullOrEmpty(vm.PrimaryEmail))
                recipients.Add(vm.PrimaryEmail);

            if (vm.SendToSecondary && !string.IsNullOrEmpty(vm.SecondaryEmail))
                recipients.Add(vm.SecondaryEmail);

            if (vm.SendToCompany && !recipients.Any())
            {
                recipients.Add(_config.GetSection("SiteConfiguration")["MailerEmail"]);
                vm.SendToCompany = false;
            }

            var attachmentContent = new MemoryStream(GetInvoicePdf(vm.InvoiceId));
            var attachment = new Attachment(attachmentContent, $"Invoice{vm.InvoiceId}.pdf", "application/pdf");

            var mailSubject = $"Your order information is ready to view! (Order #{vm.InvoiceId})";
            var mailMsg = vm.Note.Replace(Environment.NewLine, "<br />");

            foreach (var recipient in recipients)
            {
                var email = new Email(_config)
                {
                    MailSubject = mailSubject,
                    Message = mailMsg,
                    SenderEmail = _config.GetSection("SiteConfiguration")["MailerEmail"],
                    SenderDisplayName = "LNG Mailer",
                    RecipientEmail = recipient,
                    RecipientDisplayName = recipient,
                    Attachment = attachment,
                    CopyCompany = vm.SendToCompany
                };

                var error = email.SendEmail();
                if (!string.IsNullOrWhiteSpace(error))
                {
                    errorList.Add($"Email not sent - {error}");
                }
            }

            var newLog = _logService.Get(0);
            newLog.Date = DateTime.Now;
            newLog.InvoiceId = vm.InvoiceId;
            newLog.LogType = LogTypeEnum.SendInvoiceToCustomer.ToString();
            newLog.Summary =
                $"<strong>Subject:</strong> <br /> {mailSubject} <br /><br /> " +
                $"<strong>Message:</strong> <br /> {mailMsg} <br /><br /> " +
                $"<strong>Recipients:</strong> <br /> {string.Join(",", recipients)} <br /><br /> " +
                $"<strong>LNG was copied:</strong> <br /> {vm.SendToCompany} <br /><br /> " +
                $"<strong>Error Messages:</strong> <br /> {(errorList.Any() ? string.Join(",", errorList) : "none")} ";

            _logService.Add(newLog);

            if (errorList.Any())
            {
                TempData["ErrorBannerMessage"] = string.Join(',', errorList);
            }
            else
            {
                TempData["SuccessBannerMessage"] = "An email has been sent to the recipient(s) you selected.";
            }

            return RedirectToAction("ViewInvoice", new { invoiceId = vm.InvoiceId });
        }

        public byte[] GetInvoicePdf(int invoiceId)
        {
            const int itemsPerPageMax = 20;
            const string footer = "--footer-center \"Thank you for choosing LNG Laserworks, we appreciate your business!\" " +
                "--footer-line --footer-font-size \"12\" --footer-font-name \"calibri light\"";
            const string header = "--header-right \"Page [page]\" --header-font-size \"12\" --header-font-name \"calibri light\"";
            var invoice = _invoiceService.Get(invoiceId);
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
                CustomSwitches = $"{(model.TotalLineItems > itemsPerPageMax ? header : null)} {footer}"
            };

            var byteArray = actionPdf.BuildFile(ControllerContext).Result;
            return byteArray;
        }

        public FileResult GetInvoicePdfFile(int invoiceId)
        {
            return File(GetInvoicePdf(invoiceId), "application/pdf");
        }

        public IActionResult DeleteAttachment(string attachmentName = "", int invoiceId = 0)
        {
            if (System.IO.File.Exists(attachmentName))
                System.IO.File.Delete(attachmentName);

            return RedirectToAction("EditInvoice", new { invoiceId });
        }
    }
}
