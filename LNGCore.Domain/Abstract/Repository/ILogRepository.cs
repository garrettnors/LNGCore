using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface ILogRepository
    {
        void SaveLog(string logText);
    }
}
