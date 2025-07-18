using Microsoft.EntityFrameworkCore;
using RetailMate.Data; // Assuming your context is inside Data folder
using RetailMate.Models; // If Invoice and InvoiceItem are still in Models


namespace RetailMate.Data
{
    public class RetailMateContext : DbContext
    {
        public RetailMateContext(DbContextOptions<RetailMateContext> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }


    }
}
