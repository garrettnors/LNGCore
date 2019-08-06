using LNGCore.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogService _logService;

        public GlobalExceptionFilter(ILogService logService)
        {
            _logService = logService;
        }

        public void OnException(ExceptionContext context)
        {
            // log the exception
            var log = _logService.Get(0);
            log.LogType = "Exception occurred.";
            log.Summary = JsonConvert.SerializeObject(context.Exception.GetBaseException());
            log.Date = DateTime.Now;
            _logService.Add(log);
        }
    }
}
