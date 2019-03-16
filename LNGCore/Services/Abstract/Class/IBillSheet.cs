namespace LNGCore.Services.Abstract.Class
{
    public interface IBillSheet
    {
        string AdditionalInfo { get; set; }
        bool AutoDeduct { get; set; }
        decimal? CreditLimit { get; set; }
        string DueDate { get; set; }
        int Id { get; set; }
        bool? IsActive { get; set; }
        decimal? LeftToPay { get; set; }
        string Login { get; set; }
        string PaidBy { get; set; }
        bool? PaidThisMonth { get; set; }
        string Password { get; set; }
        decimal? PayAmount { get; set; }
        string WhereToPay { get; set; }
        string WhoWeOwe { get; set; }
    }
}