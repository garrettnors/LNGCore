using LNGCore.Domain.Infrastructure.PdfComponents;
using LNGCore.UI.Models.Admin;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Infrastructure
{
    public class InvoiceDocument : IDocument
    {
        public InvoicePdfModel Model { get; }

        public InvoiceDocument(InvoicePdfModel model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            var dir = Directory.GetCurrentDirectory();
            FontManager.RegisterFont(File.OpenRead($"{dir}/Fonts/Open_Sans/Static/OpenSans/OpenSans-Regular.ttf"));
            FontManager.RegisterFont(File.OpenRead($"{dir}/Fonts/Open_Sans/Static/OpenSans/OpenSans-Italic.ttf"));
            FontManager.RegisterFont(File.OpenRead($"{dir}/Fonts/Open_Sans/Static/OpenSans/OpenSans-Bold.ttf"));
            container
                .Page(page =>
                {
                    page.Size(PageSizes.Letter.Portrait());
                    page.DefaultTextStyle(TextStyle.Default.FontFamily("Open Sans"));
                    page.MarginVertical(16);
                    page.MarginHorizontal(32);
                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });

            if (Model.IncludeProofs)
            {
                var directory = $"Uploads/{Model.Invoice.Id}";
                if (Directory.Exists(directory))
                {
                    string[] files = Directory.GetFiles(directory);
                    if (files.Any())
                    {

                        foreach (var attachment in files)
                        {
                            container.Page(page => 
                            {
                                page.Size(PageSizes.Letter.Portrait());
                                page.Content().Padding(100).Image(attachment, ImageScaling.FitArea);
                            });
                        }
                    }
                }
            }            
        }
        //var image = File.OpenRead(attachment);
        //byte[] bytes = new byte[image.Length];
        //image.Read(bytes, 0, bytes.Length);
        //image.Dispose();
        //var base64 = Convert.ToBase64String(bytes, 0, bytes.Length);

        //< div class="break-after">
        //    <h1 class="text-center py-5">Attached Proof</h1>
        //    <img src = "data:image/jpg;base64,@(base64)" style= "max-width: 700px;max-height: 1000px;" class="mx-auto d-block" />
        //</div>
        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20);
            var rightSideStyle = TextStyle.Default.FontSize(12);

            container
                .Row(row =>
                {
                    row.RelativeItem(1).Column(column =>
                    {
                        column.Item().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span("LNG Laserworks");
                            text.DefaultTextStyle(titleStyle);
                        });
                        column.Item().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span("An Awards For Excellence Subsidiary");
                            text.DefaultTextStyle(TextStyle.Default.FontSize(8));
                        });
                        column.Item().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span("452 Sunset Oak, West, TX 76691");
                            text.DefaultTextStyle(TextStyle.Default.FontSize(8));
                        });
                    });
                    row.RelativeItem(1);
                    row.RelativeItem(1).Column(column =>
                    {
                        column.Item().PaddingTop(10f);
                        column.Item().Text(text =>
                        {
                            text.AlignRight();
                            text.Span("Contact Us Today!").LineHeight(1f);
                            text.DefaultTextStyle(rightSideStyle);
                        });
                        column.Item().Text(text =>
                        {
                            text.AlignRight();
                            text.Span("(254) 424-7564").LineHeight(1f);
                            text.DefaultTextStyle(rightSideStyle);
                        });
                        column.Item().Text(text =>
                        {
                            text.AlignRight();
                            text.Span("Info@LNGLaserworks.com").LineHeight(1f);
                            text.DefaultTextStyle(rightSideStyle);
                        });
                    });
                });
        }
        void ComposeOrderTotalsTable(IContainer container)
        {
            var subTotal = Model.Invoice.LineItem.Sum(s => s.Quantity * s.ItemPrice) ?? 0;
            var tax = Model.Invoice.LineItem.Sum(s => s.TaxAmount * s.Quantity);
            var total = subTotal + tax + (Model.Invoice.ShipCost ?? 0);
            var headerStyle = TextStyle.Default.Bold().FontSize(10);
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                // step 3
                table.Cell().Element(x => ElementCenterStyle(x, true)).Text("Subtotal");
                table.Cell().Element(x => ElementCenterStyle(x)).Text($"{subTotal:c}");
                table.Cell().Element(x => ElementCenterStyle(x, true)).Text("Tax");
                table.Cell().Element(x => ElementCenterStyle(x)).Text($"{tax:c}");
                table.Cell().Element(x => ElementCenterStyle(x, true)).Text("Shipping");
                table.Cell().Element(x => ElementCenterStyle(x)).Text($"{Model.Invoice.ShipCost ?? 0:c}");
                table.Cell().Element(x => ElementCenterStyle(x, true)).Text("Total");
                table.Cell().Element(x => ElementCenterStyle(x)).Text($"{total:c}");
            });
        }
        void ComposeOrderInfoTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.Bold().FontSize(10);
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(x => ElementCenterStyle(x, true)).Text("Sales Rep");
                    header.Cell().Element(x => ElementCenterStyle(x, true)).Text("PO Number");
                    header.Cell().Element(x => ElementCenterStyle(x, true)).Text("Order Date");
                    header.Cell().Element(x => ElementCenterStyle(x, true)).Text("Shipping Method");
                    header.Cell().Element(x => ElementCenterStyle(x, true)).Text("Delivery Date");
                });

                // step 3
                table.Cell().Element(x => ElementCenterStyle(x)).Text(Model.Invoice.Employee.EmpName);
                table.Cell().Element(x => ElementCenterStyle(x)).Text(Model.Invoice.Pofield);
                table.Cell().Element(x => ElementCenterStyle(x)).Text($"{Model.Invoice.OrderDate:d}");
                table.Cell().Element(x => ElementCenterStyle(x)).Text(Model.Invoice.ShippingMethod);
                table.Cell().Element(x => ElementCenterStyle(x)).Text($"{Model.Invoice.Deadline:d}");
            });
        }

        void ComposeLineItemTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.Bold().FontSize(10);
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(10);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(x => ElementLeftStyle(x, true)).Text("Qty");
                    header.Cell().Element(x => ElementLeftStyle(x, true)).Text("Item");
                    header.Cell().Element(x => ElementLeftStyle(x, true)).Text("Item Description");
                    header.Cell().Element(x => ElementRightStyle(x, true)).Text("Item Price");
                    header.Cell().Element(x => ElementRightStyle(x, true)).Text("Line Total");
                });

                // step 3
                foreach (var item in Model.Invoice.LineItem.ToList())
                {
                    table.Cell().Element(x => ElementLeftStyle(x)).Text(item.Quantity);
                    table.Cell().Element(x => ElementLeftStyle(x)).Text(item.Item.ItemName);
                    table.Cell().Element(x => ElementLeftStyle(x)).Text($"{item.ItemDesc}$");
                    table.Cell().Element(x => ElementRightStyle(x)).Text($"{item.ItemPrice:c}");
                    table.Cell().Element(x => ElementRightStyle(x)).Text($"{item.ItemPrice * item.Quantity:c}");
                }
            });
        }

        public IContainer ElementLeftStyle(IContainer container, bool isHeader = false)
        {
            if (isHeader)
                return container.Border(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(3).DefaultTextStyle(TextStyle.Default.Bold().FontSize(8));

            return container.Border(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3).PaddingLeft(3).DefaultTextStyle(TextStyle.Default.FontSize(8));
        }
        public IContainer ElementCenterStyle(IContainer container, bool isHeader = false)
        {
            if (isHeader)
                return container.Border(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(3).AlignCenter().DefaultTextStyle(TextStyle.Default.Bold().FontSize(8));

            return container.Border(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3).PaddingLeft(3).AlignCenter().DefaultTextStyle(TextStyle.Default.FontSize(8));
        }
        private IContainer ElementRightStyle(IContainer container, bool isHeader = false)
        {
            if (isHeader)
                return container.Border(1).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingRight(3).AlignRight().DefaultTextStyle(TextStyle.Default.Bold().FontSize(8));

            return container.Border(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3).PaddingRight(3).AlignRight().DefaultTextStyle(TextStyle.Default.FontSize(8));
        }

        void ComposeContent(IContainer container)
        {
            //container
            //    .PaddingVertical(40)
            //    .Height(250)
            //    .Background(Colors.Grey.Lighten3)
            //    .AlignCenter()
            //    .AlignMiddle()
            //    .Text("Content").FontSize(16);
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(1);
                column.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);                
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(rCol =>
                    {
                        rCol.Item().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span(Model.Invoice.IsQuote ? "QUOTE" : "INVOICE").LineHeight(.9f);
                            text.DefaultTextStyle(TextStyle.Default.FontSize(20));
                        });
                        rCol.Item().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span($"Number {Model.Invoice.Id}").LineHeight(1f);
                            text.DefaultTextStyle(TextStyle.Default.FontSize(12));
                        });
                        rCol.Item().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span($"Print Date: {DateTime.Now:d}").LineHeight(1f);
                            text.DefaultTextStyle(TextStyle.Default.FontSize(8));
                        });
                    });
                    row.ConstantItem(10);
                    row.RelativeItem().Component(new AddressComponent(Model.Invoice.Customer));
                    row.ConstantItem(10);
                    row.RelativeItem().Component(new AddressComponent(Model.Invoice.Customer, true));
                });
                column.Item().PaddingBottom(10);
                column.Item().Element(ComposeOrderInfoTable);
                column.Item().PaddingBottom(10);
                column.Item().Element(ComposeLineItemTable);
                column.Item().PaddingBottom(10);
                column.Item().Row(row =>
                {
                    row.RelativeItem(18).PaddingTop(10).DefaultTextStyle(TextStyle.Default.FontSize(10)).Column(col =>
                    {
                        col.Item().AlignCenter().Text("Please make checks");
                        col.Item().AlignCenter().Text("payable to");
                        col.Item().AlignCenter().Text("\"Awards For Excellence\"");
                        col.Item().AlignCenter().Text("LNG is an Awards For Excellence Subsidiary").FontSize(6);
                    });
                    row.RelativeItem(8).Element(ComposeOrderTotalsTable);
                });
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.Column(column => 
            {
                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                column.Item().AlignCenter().Text("Thank you for choosing LNG Laserworks, we appreciate your business!").FontSize(10f);
            });
        }
    }
}
