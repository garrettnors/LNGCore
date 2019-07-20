using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationService _configService;
        public ConfigurationController(IConfigurationService configService)
        {
            _configService = configService;
        }
        public IActionResult Index()
        {
            var useDarkMode = false;
            var darkModeCookie = Request.Cookies["SetDarkModeEnabled"];
            if (darkModeCookie != null)
                useDarkMode = bool.Parse(Request.Cookies["SetDarkModeEnabled"]);

            var vm = new ConfigurationViewModel
            {
                UseDarkMode = useDarkMode,
                BgColor = _configService.GetConfigurationValueByName("LightModeBgColor")
            };

            return View(vm);
        }

        public IActionResult SetDarkMode(bool useDarkMode)
        {
            var options = new CookieOptions();
            options.Expires = DateTime.Now.AddYears(5);
            Response.Cookies.Append("SetDarkModeEnabled", useDarkMode.ToString());
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetLightModeBgColor(ConfigurationViewModel model)
        {
            _configService.SaveConfigurationByName("LightModeBgColor", model.BgColor);
            return RedirectToAction("Index");
        }

        public IActionResult LightBgStylesheet()
        {
            var bgColor = _configService.GetConfigurationValueByName("LightModeBgColor");
            return Content("body { background-color:" + bgColor + "; }", "text/css");
        }
    }
}