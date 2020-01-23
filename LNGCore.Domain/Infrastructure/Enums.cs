using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Infrastructure
{
    public class Enums
    {
        public enum InvoiceTypeEnum
        {
            Open,
            Paid,
            Quote,
            Voided,
            Donated,
            PastDue,
            All
        }

        public enum InvoiceReportTypeEnum
        {
            Sales,
            NonTaxSales,
            Donated,
            Shipping
        }
        public enum LogTypeEnum
        {
            SendInvoiceToCustomer,
            Error
        }
    }
}
