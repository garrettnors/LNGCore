using LNGCore.Services.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Services.Abstract.Repository
{
    public interface ILogRepository
    {
        ILog GetLog(int logId);
        void SaveLog(ILog log);
    }
}
