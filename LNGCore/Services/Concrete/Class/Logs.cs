using System;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{

    public partial class Logs : ILogs
    {
        public int Id { get; set; }
        public string Log { get; set; }
        public DateTime Date { get; set; }
    }
}
