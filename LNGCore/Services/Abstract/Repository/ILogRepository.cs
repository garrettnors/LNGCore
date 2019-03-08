using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface ILogRepository
    {
        ILog GetLog(int logId);
        void SaveLog(ILog log);
    }
}
