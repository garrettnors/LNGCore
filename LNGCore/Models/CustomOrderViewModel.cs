using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LNGCore.UI.Models
{
    public class CustomOrderViewModel
    {
        public int Id { get; set; }
        public string OrnamentShape { get; set; }
        public string OrnamentDesign { get; set; }
        public string SpecialInstructions { get; set; }
        public IFormFile UploadFile { get; set; }
        public int Amount { get; set; }
    }
}
