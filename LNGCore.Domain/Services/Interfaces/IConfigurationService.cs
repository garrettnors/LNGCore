using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IConfigurationService
    {
        string GetConfigurationValueByName(string name);
        void SaveConfigurationByName(string name, string value);
    }
}
