using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetailMate.Models
{
    public class Invoice
    {
        [Required(ErrorMessage = "Customer Name is required")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Billing Address is required")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string BillNumber { get; set; }

        public string ShopName { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();
    }
}
