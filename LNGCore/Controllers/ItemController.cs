using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        public IActionResult Index(int page = 1, int take = 40, string searchTerm = "")
        {
            ViewBag.ActiveAction = ControllerContext.RouteData.Values["controller"];

            var vm = new ItemViewModel();

            var skip = take * (page - 1);
            var pagination = new PaginationViewModel
            {
                Take = take,
                CurrentPage = skip / take + 1
            };

            var items = _itemService.GetAll(searchTerm).ToList();
            pagination.NumberOfPages =
                items.Count <= take ? 1 : (int)Math.Ceiling(items.Count / (decimal)take);

            vm.Items = items.OrderBy(o => o.ItemName).Skip(skip).Take(take).ToList();
            vm.PaginationParameters = pagination;
            vm.SearchTerm = searchTerm;

            return View(vm);
        }
        [HttpGet]
        public IActionResult EditItem(int itemId)
        {
            var vm = _itemService.Get(itemId);
            return PartialView("_EditItem", vm);
        }

        [HttpPost]
        public IActionResult EditItem(Item item)
        {
            if (item.ItemId == 0)
            {
                TempData["SuccessBannerMessage"] = "Item successfully added.";
                _itemService.Add(item);
            }
            else
            {
                TempData["SuccessBannerMessage"] = "Item successfully updated.";
                _itemService.Edit(item);
            }
                       
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(int itemId)
        {
            _itemService.Delete(itemId);
            TempData["SuccessBannerMessage"] = "Item successfully deleted.";
            return RedirectToAction("Index");
        }
    }
}