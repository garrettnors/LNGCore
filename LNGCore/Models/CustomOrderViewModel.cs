using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Services.Abstract.Class;
using Microsoft.AspNetCore.Http;

namespace LNGCore.UI.Models
{

    public class CustomOrderViewModel
    {
        public CustomOrderViewModel()
        {
            ExistingOrders = new List<NewOrnamentOrder>();
        }

        public string TotalCost
        {
            get
            {
                var total = 0;

                var defaultRate = 6;
                var discountRate = 5;

                var orderCount = ExistingOrders.Sum(s => s.Amount);

                while (orderCount >= 3)
                {
                    total += discountRate * 3;
                    orderCount = orderCount - 3;
                }

                total += orderCount * defaultRate;
                return $"{total:C}";
            }
        }

        public NewOrnamentOrder NewOrder { get; set; }
        public List<NewOrnamentOrder> ExistingOrders { get; set; }
        public string GooglePublicKey { get; set; }
    }
    public class NewOrnamentOrder : IOrnamentOrders
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a style.")]
        public string OrnamentStyle { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a design.")]
        public string OrnamentDesign { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please describe what your ornament should look like.")]
        public string SpecialInstructions { get; set; }
        [Required(ErrorMessage = "Please select between 1 and 99")]
        [Range(1, 99)]
        public int Amount { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "You must use a valid email address so that we can contact you about your order.")]
        public string UserEmail { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name.")]
        public string UserName { get; set; }

    }
}
