using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{
    public partial class BillSheet : IBillSheet
    {
        public int Id { get; set; }
        public string WhoWeOwe { get; set; }
        public string WhereToPay { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DueDate { get; set; }
        public string PaidBy { get; set; }
        public decimal? PayAmount { get; set; }
        public decimal? LeftToPay { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool? PaidThisMonth { get; set; }
        public string AdditionalInfo { get; set; }
        public bool? IsActive { get; set; }
        public bool AutoDeduct { get; set; }
    }
}
