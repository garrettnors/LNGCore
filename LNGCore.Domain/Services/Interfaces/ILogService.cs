using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface ILogService
    {
        Log GetLog(int logId);
        void SaveLog(Log log);
    }

}
