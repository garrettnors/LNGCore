﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    public class PriceListController : Controller
    {
        private readonly IPriceListService _priceListService;
        public PriceListController(IPriceListService priceListService)
        {
            _priceListService = priceListService;
        }
        public IActionResult Index(int page = 1, int take = 15, string searchTerm = "")
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

            vm.Prices = priceList.Skip(skip).Take(take).ToList();
            vm.PaginationParameters = pagination;
            vm.SearchTerm = searchTerm;

            return View(vm);
        }

        [HttpGet]
        public IActionResult EditPrice(int itemId)
        {
            var price = _priceListService.Get(itemId);
            return PartialView("_EditPrice", price);
        }

        [HttpPost]
        public IActionResult EditPrice(PriceList price)
        {
            if (price.Id == 0)
            {
                TempData["BannerSuccessMessage"] = "Price successfully added.";
                _priceListService.Add(price);
            }
            else
            {
                TempData["BannerSuccessMessage"] = "Price successfully updated.";
                _priceListService.Edit(price);
            }
                       
            return RedirectToAction("Index");
        }

        public IActionResult DeletePrice(int priceId)
        {
            _priceListService.Delete(priceId);
            TempData["BannerSuccessMessage"] = "Price successfully deleted.";
            return RedirectToAction("Index");
        }
    }
}