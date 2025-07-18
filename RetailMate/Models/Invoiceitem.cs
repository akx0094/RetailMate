// File: Models/InvoiceItem.cs
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailMate.Models
{
    public class InvoiceItem
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
    }
}