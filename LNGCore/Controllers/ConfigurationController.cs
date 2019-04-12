using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LNGCore.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            var useDarkMode = false;
            var darkModeCookie = Request.Cookies["SetDarkModeEnabled"];
            if (darkModeCookie != null)
                useDarkMode = bool.Parse(Request.Cookies["SetDarkModeEnabled"]);

            return View(useDarkMode);
        }

        public IActionResult SetDarkMode(bool useDarkMode)
        {
            var options = new CookieOptions();
            options.Expires = DateTime.Now.AddYears(5);
            Response.Cookies.Append("SetDarkModeEnabled", useDarkMode.ToString());
            return RedirectToAction("Index");
        }
    }
}