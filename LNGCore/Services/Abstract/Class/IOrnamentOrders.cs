using System.ComponentModel.DataAnnotations;

namespace LNGCore.Domain.Abstract.Class
{
    public interface IOrnamentOrders
    {
        int Id { get; set; }
        string OrnamentStyle { get; set; }
        string OrnamentDesign { get; set; }
        string SpecialInstructions { get; set; }
        int Amount { get; set; }
        string UserEmail { get; set; }
        string UserName { get; set; }
    }
}