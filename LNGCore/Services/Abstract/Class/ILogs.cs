using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Class
{
    public interface ILogs
    {
        int Id { get; set; }
        string Log { get; set; }
        DateTime Date { get; set; }
    }
}
