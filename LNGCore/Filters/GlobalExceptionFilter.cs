using LNGCore.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IConfiguration _configuration;
        public GlobalExceptionFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnException(ExceptionContext context)
        {
            var errMsg = string.Join("-----", context.Exception.InnerException) + " \n\r " + context.Exception.StackTrace;

            using (var conn = new SqlConnection(_configuration.GetSection("SiteConfiguration")["DbContext"]))
            {
                var cmd = "INSERT INTO [LOG] ([LogType], [Summary], [Date]) VALUES (@logType, @summary, GETDATE())";
                using (var command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.Add(new SqlParameter("@logType", "Exception Occured."));
                    command.Parameters.Add(new SqlParameter("@summary", errMsg.Length > 8000 ? errMsg.Substring(0,8000) : errMsg));
                    if (conn.State.Equals(ConnectionState.Closed))
                    {
                        conn.Open();
                    }
                    command.ExecuteNonQuery();
                }
                if (conn.State.Equals(ConnectionState.Open))
                {
                    conn.Close();
                }
            }

            // log the exception
            //var log = _logService.Get(0);
            //log.LogType = "Exception occurred.";
            //log.Summary = "something";
            //log.Date = DateTime.Now;
            //_logService.Add(log);
        }
    }
}
