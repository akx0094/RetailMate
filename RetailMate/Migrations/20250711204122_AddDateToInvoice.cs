using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailMate.Migrations
{
    /// <inheritdoc />
    public partial class AddDateToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Invoices",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Invoices",
                newName: "InvoiceDate");
        }
    }
}
