using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class BillDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionCurrency",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionPaymentGatewayResponse",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionPaymentReferenceId",
                table: "PaymentTransactions");

            migrationBuilder.AddColumn<string>(
                name: "BillDetails",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillDetails",
                table: "Bills");

            migrationBuilder.AddColumn<string>(
                name: "TransactionCurrency",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionPaymentGatewayResponse",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionPaymentReferenceId",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
