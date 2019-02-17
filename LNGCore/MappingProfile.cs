using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LNGCore.Domain.Abstract.Class;
using LNGCore.UI.Models.Admin;

namespace LNGCore.UI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<InvoiceItem, IInvoice>();
        }
    }
}
