using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionPaymentGatewayTransactionAmount",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionPaymentGatewayTransactionId",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "TransactionPaymentStatus",
                table: "PaymentTransactions",
                newName: "TransactionStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionStatus",
                table: "PaymentTransactions",
                newName: "TransactionPaymentStatus");

            migrationBuilder.AddColumn<string>(
                name: "TransactionPaymentGatewayTransactionAmount",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionPaymentGatewayTransactionId",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
