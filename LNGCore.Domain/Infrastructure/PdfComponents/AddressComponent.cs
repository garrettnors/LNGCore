using LNGCore.Domain.Database;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Infrastructure.PdfComponents
{
    public class AddressComponent : IComponent
    {
        private bool IsShipTo { get; }
        private Customer Customer { get; }

        public AddressComponent(Customer customer, bool isShipto = false)
        {
            IsShipTo = isShipto;
            Customer = customer;
        }

        public void Compose(IContainer container)
        {
            container
                .DefaultTextStyle(TextStyle.Default.FontSize(8))
                .Row(row =>
                {
                    row.RelativeItem(2).Column(column => 
                    {
                        if (IsShipTo)
                        {
                            column.Item().AlignRight().Text("SHIP").SemiBold();
                            column.Item().AlignRight().Text("TO").SemiBold();
                        }
                        else
                        {
                            column.Item().AlignRight().Text("TO").SemiBold();
                        }                        
                    });
                    row.RelativeItem(1).PaddingHorizontal(5).LineVertical(1).LineColor(Colors.Grey.Lighten3);
                    row.RelativeItem(9).Column(column => 
                    {
                        column.Item().Text(Customer.Name);

                        if (!string.IsNullOrWhiteSpace(Customer.BusinessName))
                            column.Item().Text(Customer.BusinessName);

                        column.Item().Text($"{Customer.Street}");
                        column.Item().Text($"{Customer.City}, {Customer.State} {Customer.ZipCode}");
                    });

                });
        }

        //public void Compose(IContainer container)
        //{
        //    container
        //        .DefaultTextStyle(TextStyle.Default.FontSize(8))
        //        .Column(column =>
        //        {
        //            column.Spacing(2);

        //            if (IsShipTo)
        //            {
        //                column.Item().AlignRight().Text("SHIP").LineHeight(.5f).SemiBold();
        //                column.Item().BorderBottom(1).PaddingBottom(5).AlignRight().Text("TO").SemiBold();
        //            }
        //            else
        //            {
        //                column.Item().BorderBottom(1).PaddingBottom(5).AlignRight().Text("TO").SemiBold();
        //            }

        //            column.Item().Text(Customer.Name);

        //            if (!string.IsNullOrWhiteSpace(Customer.BusinessName))
        //                column.Item().Text(Customer.BusinessName);

        //            column.Item().Text($"{Customer.City}, {Customer.State}");
        //            column.Item().Text(Customer.Email);
        //            column.Item().Text(Customer.BusinessPhone ?? Customer.AltPhone);
        //        });
        //}
    }
}
