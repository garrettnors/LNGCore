using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PriceListController : Controller
    {
        private readonly IPriceListService _priceListService;
        private readonly IInvoiceService _invoiceService;
        public PriceListController(IPriceListService priceListService, IInvoiceService invoiceService)
        {
            _priceListService = priceListService;
            _invoiceService = invoiceService;
        }
        public IActionResult Index(int page = 1, int take = 25, string searchTerm = "")
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["controller"];

            var vm = new PriceListViewModel();

            var skip = take * (page - 1);
            var pagination = new PaginationViewModel
            {
                Take = take,
                CurrentPage = skip / take + 1
            };

            var priceList = _priceListService.GetAll(searchTerm).ToList();
            pagination.NumberOfPages =
                priceList.Count <= take ? 1 : (int)Math.Ceiling(priceList.Count / (decimal)take);

            vm.Prices = priceList.OrderBy(b => b.ItemType.ItemName).ThenBy(t => t.ItemNumber).Skip(skip).Take(take).ToList();
            vm.PaginationParameters = pagination;
            vm.SearchTerm = searchTerm;

            return View(vm);
        }

        [HttpGet]
        public IActionResult EditPrice(int itemId)
        {
            var vm = new EditPriceViewModel
            {
                Price = _priceListService.Get(itemId),
                Items = _invoiceService.GetItemTypes().ToList()           
            };
            return PartialView("_EditPrice", vm);
        }

        [HttpPost]
        public IActionResult EditPrice(PriceList price, bool keepModalOpen = false)
        {
            if (price.Id == 0)
            {
                TempData["SuccessBannerMessage"] = "Price successfully added.";
                _priceListService.Add(price);
            }
            else
            {
                TempData["SuccessBannerMessage"] = "Price successfully updated.";
                _priceListService.Edit(price);
            }

            TempData["KeepModalOpenForId"] = keepModalOpen ? (int?)price.Id : null;
            return RedirectToAction("Index");
        }

        public IActionResult DeletePrice(int priceId)
        {
            _priceListService.Delete(priceId);
            TempData["SuccessBannerMessage"] = "Price successfully deleted.";
            return RedirectToAction("Index");
        }
    }
}