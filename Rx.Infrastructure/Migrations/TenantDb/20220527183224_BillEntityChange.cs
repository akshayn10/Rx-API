using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class BillEntityChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Subscriptions_SubscriptionId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Bills_BillId",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "BillId",
                table: "PaymentTransactions",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_BillId",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_SubscriptionId");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "Bills",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_SubscriptionId",
                table: "Bills",
                newName: "IX_Bills_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_OrganizationCustomers_CustomerId",
                table: "Bills",
                column: "CustomerId",
                principalTable: "OrganizationCustomers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Subscriptions_SubscriptionId",
                table: "PaymentTransactions",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_OrganizationCustomers_CustomerId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Subscriptions_SubscriptionId",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "PaymentTransactions",
                newName: "BillId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_SubscriptionId",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_BillId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Bills",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_CustomerId",
                table: "Bills",
                newName: "IX_Bills_SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Subscriptions_SubscriptionId",
                table: "Bills",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Bills_BillId",
                table: "PaymentTransactions",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
