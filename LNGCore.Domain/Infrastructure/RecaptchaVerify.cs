using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LNGCore.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LNGCore.Services.Logical
{
    public class RecaptchaVerify
    {
        private readonly ILogService _logService;
        private readonly IConfiguration _config;
        public RecaptchaVerify(IConfiguration configParam, ILogService logRepoParam)
        {
            _config = configParam;
            _logService = logRepoParam;
        }

        public async Task<GoogleResponseObject> GetRecaptchaScore(string googleClientToken)
        {
            using (var httpClient = new HttpClient())
            {
                var secret = _config.GetSection("SiteConfiguration")["RecaptchaSiteSecret"];
                var response = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?response={googleClientToken}&secret={secret}").Result;
                var jsonString = await response.Content.ReadAsStringAsync();
                var log = _logService.GetLog(0);
                log.Date = DateTime.Now;
                log.LogType = "Error";
                log.Summary = $"Recaptcha Response: {jsonString}";
                _logService.SaveLog(log);
                return JsonConvert.DeserializeObject<GoogleResponseObject>(jsonString);
            }
        }
    }
    public class GoogleResponseObject
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
