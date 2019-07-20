using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly LngDbContext _db;
        public ConfigurationService(LngDbContext db)
        {
            _db = db;
        }

        public string GetConfigurationValueByName(string name)
        {
            return _db.Configuration.FirstOrDefault(f => f.Name == name)?.Value;
        }

        public void SaveConfigurationByName(string name, string value)
        {
            var config = _db.Configuration.FirstOrDefault(f => f.Name == name);
            if (config != null)
            {
                config.Value = value;
                _db.SaveChanges();
            }
        }
    }
}
